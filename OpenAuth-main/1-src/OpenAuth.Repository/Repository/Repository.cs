using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions; 

namespace OpenAuth.Repository
{
    public abstract class Repository<TDbContext, T> : IRepository<TDbContext, T>
        where T : class, new()
        where TDbContext : DbContext
    {
        private TDbContext _context;

        private IAuthService _authService;

        public TDbContext DbContext { get { return _context; } }

        public IAuthService AuthService { get { return _authService; } }

        public NLog.ILogger Logger = NLog.LogManager.GetCurrentClassLogger();

        public Repository(TDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }
        public virtual DbSet<T> Table => _context.Set<T>();

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Insert(T entity)
        {
            var id = ReflectHelper.GetValue(entity, "Id");
            if (id == null)
            {
                id = IdGenerator.NextId();
                ReflectHelper.SetValue(entity, "Id", id.ToString());
            }

            
            if (_authService.UserInfo != null)
            {
                //设置create
                ReflectHelper.SetValue(entity, "CreateTime", DateTime.Now);
                ReflectHelper.SetValue(entity, "CreateBy", _authService.UserInfo.Id);
                ReflectHelper.SetValue(entity, "TenantId", _authService.UserInfo.TenantId);
            }
            //软删除
            ReflectHelper.SetValue(entity, "Deleted", 0);
            _context.Entry(entity).State = EntityState.Added;
        }
         
        public void Update(T entity)
        {
            if (_authService.UserInfo != null)
            {
                //设置create
                ReflectHelper.SetValue(entity, "UpdateTime", DateTime.Now);
                ReflectHelper.SetValue(entity, "UpdateBy", _authService.UserInfo.Id); 
            }
            //AttachIfNot(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            if (entity.GetType().GetProperty("Deleted") != null)
            { 
                ReflectHelper.SetValue(entity, "Deleted", 1);
                Update(entity);
            }
            else 
            {
                //AttachIfNot(entity);
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public void Delete(object id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public void Delete(Expression<Func<T, bool>> expression)
        {
            var list = Queryable(expression).ToList();
            foreach (var item in list)
            {
                Delete(item);
            }
        }

        public T Find(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public T Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).FirstOrDefault();
        }

        public IQueryable<T> Queryable()
        {
            return _context.Set<T>();
        }

        public IQueryable<T> Queryable(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public List<T> List()
        {
            return _context.Set<T>().ToList();
        }

        public List<T> List(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).ToList();
        }

        public List<T> List(Page page)
        {
            var query = _context.Set<T>().AsQueryable();
            page.total = query.Count();
            return query.ToPage(page);
        }

        public List<T> List(Expression<Func<T, bool>> expression, Page page)
        {
            var query = _context.Set<T>().Where(expression);
            page.total = query.Count();
            var list = query.ToPage(page); 
            return list;
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public int Count(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).Count();
        }


        public F Find<F>(object id) where F : class, new()
        {
            return _context.Set<F>().Find(id);
        }

        public F Find<F>(Expression<Func<F, bool>> expression) where F : class, new()
        {
            return _context.Set<F>().Where(expression).FirstOrDefault();
        }

        public IQueryable<F> Queryable<F>() where F : class, new()
        {
            return _context.Set<F>();
        }

        public IQueryable<F> Queryable<F>(Expression<Func<F, bool>> expression) where F : class, new()
        {
            return _context.Set<F>().Where(expression);
        }

        public List<F> List<F>() where F : class, new()
        {
            return _context.Set<F>().ToList();
        }

        public List<F> List<F>(Expression<Func<F, bool>> expression) where F : class, new()
        {
            return _context.Set<F>().Where(expression).ToList();
        }

        public List<F> List<F>(Page page) where F : class, new()
        {
            var query = _context.Set<F>().AsQueryable();
            page.total = query.Count();
            return query.ToPage(page);
        }

        public List<F> List<F>(Expression<Func<F, bool>> expression, Page page) where F : class, new()
        {
            var query = _context.Set<F>().Where(expression);
            page.total = query.Count();
            return query.ToPage(page);
        }

        public int Count<F>() where F : class, new()
        {
            return _context.Set<F>().Count();
        }

        public int Count<F>(Expression<Func<F, bool>> expression) where F : class, new()
        {
            return _context.Set<F>().Where(expression).Count();
        }

        public void BeginTransaction(Action action)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, 
                new TransactionOptions() 
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromSeconds(30) // 设置适当的超时
                }))
            {
                try
                {
                    //执行
                    action(); 
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
        }

        public async Task BeginTransaction(Func<Task> asyncAction  )
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions() 
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromSeconds(30) // 设置适当的超时
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                //执行
                await asyncAction();
                scope.Complete();
            }
        }

        public List<T> List(string sqlText, params DbParameter[] parameters)
        {
            using var conn = OpenConnection();
            LoggerSql(sqlText, parameters);
            return conn.Query<T>(sqlText, parameters).ToList();
        }

        public List<F> List<F>(string sqlText, params DbParameter[] parameters) where F: class, new()
        {
            using var conn = OpenConnection();
            LoggerSql(sqlText, parameters);
            return conn.Query<F>(sqlText, parameters).ToList();
        }

        public List<T> List(string sqlText, Page page, params DbParameter[] parameters)
        {
            using var conn = OpenConnection(); 
            var total = conn.ExecuteScalar(GetTotalQueryString(sqlText), parameters).ToInt(0);
            page.total = total;

            var sql = GetPageQueryString(sqlText, page);
            LoggerSql(sql, parameters);
            return conn.Query<T>(sql, parameters).ToList();
        }

        public List<F> List<F>(string sqlText, Page page,params DbParameter[] parameters) where F : class, new()
        {
            using var conn = OpenConnection();
            var total = conn.ExecuteScalar(GetTotalQueryString(sqlText), parameters).ToInt(0);
            page.total = total;

            var sql = GetPageQueryString(sqlText, page);
            LoggerSql(sql, parameters);
            return conn.Query<F>(sql, parameters).ToList();
        }

        private DbConnection _Connection;

        private DbConnection OpenConnection()
        {
            if (_Connection == null || _Connection.State != System.Data.ConnectionState.Open)
            {
                var connectionStirng = InternalApp.Configuration.GetConnectionString("DefaultDB");
                _Connection = new SqlConnection(connectionStirng);
                _Connection.Open();
            }
            return _Connection;
        }

        private string GetTotalQueryString(string sqlText)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT COUNT(*) FROM (").Append(sqlText).Append(")T ");
            return sb.ToString();
        }

        private string GetPageQueryString(string sqlText, Page page)
        {
            StringBuilder sbOrderBy = new StringBuilder();
            if (page.sorters != null && page.sorters.Count > 0)
            {
                sbOrderBy.Append(" ORDER BY ");
                foreach (var sorter in page.sorters)
                {
                    sbOrderBy.Append(sorter.sidx).Append(" ").Append(sorter.sord).Append(" , ");
                }
                sbOrderBy.Remove(sbOrderBy.Length - 1, 1);
            }
            else
            {
                if (page.sidx.IsEmpty())
                {
                    sbOrderBy.Append(" ORDER BY Id ASC ");
                }
                else
                {
                    sbOrderBy.Append(" ORDER BY ").Append(page.sidx).Append(" ").Append(page.sord).Append(" ");
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM (").
              Append("SELECT * , ROW_NUMBER() OVER( ").Append(sbOrderBy).Append(") rowNumber")
                .Append(" FROM (").Append(sqlText).Append(" )T ")
                .Append(" )T WHERE rowNumber>").Append(page.rows * (page.current - 1))
                .Append(" AND rowNumber<=").Append(page.rows * page.current);
            return sb.ToString();
        }


        private void LoggerSql(string sqlText, params DbParameter[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("查询语句:").Append(sqlText).Append(";");
            if (parameters != null && parameters.Length > 0)
            {
                sb.Append("查询参数:[");

                foreach (var param in parameters)
                {
                    sb.Append(param.ParameterName).Append(":").Append(param.Value.ToStr(""));
                }
                sb.Append("]");
            }
            Logger.Debug(sb.ToString());
        }

    }
}

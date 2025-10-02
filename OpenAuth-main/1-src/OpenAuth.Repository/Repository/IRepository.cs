using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace OpenAuth.Repository
{
    public interface IRepository<TDbContext,T>
        where T: class, new()
        where TDbContext : DbContext
    {

        TDbContext DbContext { get; }

        IAuthService AuthService { get; }

        int SaveChanges();

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(object id);

        void Delete(Expression<Func<T, bool>> expression);

        T Find(object id);

        T Find(Expression<Func<T, bool>> expression);

        IQueryable<T> Queryable();

        IQueryable<T> Queryable(Expression<Func<T, bool>> expression);

        List<T> List();

        List<T> List(Expression<Func<T, bool>> expression);

        List<T> List(Page page);

        List<T> List(Expression<Func<T, bool>> expression, Page page);

        int Count();

        int Count(Expression<Func<T, bool>> expression);

        F Find<F>(object id) where F : class, new();

        F Find<F>(Expression<Func<F, bool>> expression) where F : class, new();

        IQueryable<F> Queryable<F>() where F : class, new();

        IQueryable<F> Queryable<F>(Expression<Func<F, bool>> expression) where F : class, new();

        List<F> List<F>() where F : class, new();

        List<F> List<F> (Expression<Func<F, bool>> expression) where F : class, new();

        List<F> List<F>(Page page) where F : class, new();

        List<F> List<F>(Expression<Func<F, bool>> expression, Page page) where F : class, new();

        int Count<F>() where F : class, new();

        int Count<F>(Expression<Func<F, bool>> expression) where F : class, new();


        void BeginTransaction(Action action);

        Task BeginTransaction(Func<Task> asyncAction);

        List<T> List(string sqlText, params DbParameter[] parameters);

        List<F> List<F>(string sqlText, params DbParameter[] parameters) where F : class, new();

        List<T> List(string sqlText, Page page, params DbParameter[] parameters);

        List<F> List<F>(string sqlText, Page page, params DbParameter[] parameters) where F : class, new();
    }

   
}

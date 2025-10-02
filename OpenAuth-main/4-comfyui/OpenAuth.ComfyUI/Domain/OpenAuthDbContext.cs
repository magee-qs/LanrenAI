
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Domain.Sys; 
using System.Linq.Expressions;


namespace OpenAuth.ComfyUI.Domain
{
    public partial class OpenAuthDbContext : DbContext
    {
        private ILoggerFactory _LoggerFactory; 
        
        private IConfiguration _Configuration;
        public OpenAuthDbContext(ILoggerFactory loggerFactory, IConfiguration configuration) 
        {
            _LoggerFactory = loggerFactory;
            _Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var logger = _LoggerFactory.CreateLogger<OpenAuthDbContext>();
           
            var conn = _Configuration.GetConnectionString("DefaultDB");
            logger.LogInformation("加载DbContext:" + conn);
            optionsBuilder.UseLoggerFactory(_LoggerFactory);

            //允许打印参数
            optionsBuilder.UseSqlServer(conn)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
           
            InitTenant(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureFilters(modelBuilder);
        }


        protected virtual void ConfigureFilters(ModelBuilder modelBuilder)
        {
             
            //软删除
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {

                var prop = entity.GetProperties().Where(t => t.Name == "Deleted").FirstOrDefault();
                if (prop != null)
                {
                    var parameter = Expression.Parameter(entity.ClrType, "e");
                    var property = Expression.Property(parameter, "Deleted");
                    var filter = Expression.Lambda(
                        Expression.Equal(property, Expression.Constant(0)),
                        parameter);

                    entity.SetQueryFilter(filter);
                }
                
            }
        }


        //初始化多租户信息，根据租户id调整数据库
        private void InitTenant(DbContextOptionsBuilder optionsBuilder)
        {
            
        }


        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<FlowEntity> Flows { get; set; }
        public virtual DbSet<TaskEntity> Tasks { get; set; }
        

        public virtual DbSet<FeeLevelEntity> FeeLevels { get; set; }
        public virtual DbSet<FeeOrderEntity> FeeOrders { get; set; }

        public virtual DbSet<FeeUserEntity> FeeUsers { get; set; }

        public virtual DbSet<CostEntity> Costs { get; set; } 
        public virtual DbSet<CostListEntity> CostsList { get; set; }
        public virtual DbSet<JobEntity> Jobs { get; set; }
        public virtual DbSet<TaskFileEntity> TaskFiles { get; set; }


       
        
    }
}

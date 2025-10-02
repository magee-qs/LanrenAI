using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Service
{
    public abstract class Service<TDbContext, T> : Repository<TDbContext, T>, IService<TDbContext, T>
         where T : class, new()
         where TDbContext : DbContext
    {
        protected Service(TDbContext context, IAuthService authService) 
            : base(context, authService)
        {

        }
    }
}

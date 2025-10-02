using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.Repository.Service
{
    public interface IService<TDbContext, T>: IRepository<TDbContext, T>
        where T : class, new()
        where TDbContext : DbContext
    {
        
    }
}

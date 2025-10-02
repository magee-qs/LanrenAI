using OpenAuth.ComfyUI.Domain;
using OpenAuth.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Repository
{
    public interface IBaseRepository<T> : IRepository<OpenAuthDbContext, T> where T : class, new()
    {

    }

    public class BaseRepository<T> : Repository<OpenAuthDbContext, T> , IBaseRepository<T> where T : class, new()
    {
        public BaseRepository(OpenAuthDbContext context, IAuthService authService) 
            : base(context, authService)
        {
        }
    }
}

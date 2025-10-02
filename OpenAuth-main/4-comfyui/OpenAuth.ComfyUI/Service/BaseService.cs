using Microsoft.Extensions.Logging;
using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Service.ComfyUI;
using OpenAuth.Repository.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service
{
    public class BaseService<T> : Service<OpenAuthDbContext, T> , IBaseService<T> where T : class, new()
    {
        public ILogger<BaseService<T>> logger { get; set; }
        public BaseService(OpenAuthDbContext context, IAuthService authService) 
            : base(context, authService)
        {
            logger = AutofacServiceProvider.GetService<ILogger<BaseService<T>>>();
        }
    }

    public interface IBaseService<T> : IService<OpenAuthDbContext, T> where T : class, new()
    {

    }
}

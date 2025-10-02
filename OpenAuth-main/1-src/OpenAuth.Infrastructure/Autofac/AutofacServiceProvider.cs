using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class AutofacServiceProvider
    {
        public static IConfiguration Configuration { get; private set; }
        public static  IServiceProvider ServiceProvider { get; private set; }

        public static void ConfigureServiceProvider(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            Configuration = configuration;
            ServiceProvider = serviceProvider;
        }


        public static TService GetService<TService>() where TService : class
        {
            Type type = typeof(TService);
            return (TService)ServiceProvider.GetService(type);
        }

        public static object? GetService(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }
    }
}

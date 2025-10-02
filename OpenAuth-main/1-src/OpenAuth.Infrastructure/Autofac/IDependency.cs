using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    /// <summary>
    /// autofac注入基类
    /// </summary>
    public interface IDependency
    {
    }

    /// <summary>
    /// 单例模式
    /// </summary>
    public interface ISingletonDependency : IDependency
    {

    }

    /// <summary>
    /// 实例模式
    /// </summary>
    public interface IScopeDependency : IDependency
    {

    }
}

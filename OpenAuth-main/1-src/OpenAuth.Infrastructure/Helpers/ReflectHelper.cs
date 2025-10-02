using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class ReflectHelper
    {
        private static List<Assembly> _assemblyList = new List<Assembly>();

        private static object _lock = new object();

        private static List<Assembly> LoadAssembly()
        {
            // 动态编译库
            var compilationLibrary = DependencyContext.Default
                .CompileLibraries
                .Where(x => !x.Serviceable
                            && x.Type == "project").ToList();

            var count1 = compilationLibrary.Count;
            List<Assembly> assemblyList = new List<Assembly>();

            foreach (var _compilation in compilationLibrary)
            {
                try
                {
                    assemblyList.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(_compilation.Name)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(_compilation.Name + ex.Message);
                }
            }

            return assemblyList;
        }

        /// <summary>
        /// 获取动态库
        /// </summary>
        public static List<Assembly> AssemblyList
        {
            get
            {
                if (_assemblyList == null || _assemblyList.Count == 0)
                {
                    lock (_lock)
                    {
                        if (_assemblyList == null || _assemblyList.Count == 0)
                        {
                            _assemblyList = LoadAssembly();
                        }
                    }
                }

                return _assemblyList;
            }
        }


        /// <summary>
        /// 读取字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static object GetValue<T>(T entity, string property)
        {
            if (entity == null) return null;
            try
            {
                return entity.GetType().GetProperty(property)?.GetValue(entity);
            }
            catch
            {
                return null;
            }
        }

        public static void SetValue<T>(T entity, string property, object value)
        {
            if (entity == null) return;
            try
            {
                entity.GetType().GetProperty(property)?.SetValue(entity, value);
            }
            catch
            {
                return;
            }
        } 
    }
}

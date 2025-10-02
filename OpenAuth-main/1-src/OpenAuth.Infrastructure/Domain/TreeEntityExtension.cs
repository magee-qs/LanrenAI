using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public static class TreeEntityExtension
    {

        private static string parentId = "ParentId";

        private static string Key = "Id";

        /// <summary>
        /// 转为树形 必须包括Id, ParentId , Children三个字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<T> ToTree<T>(this List<T> list, object parentId = null)
        {
            var tree = new List<T>();

            if (list == null || list.Count == 0)
                return tree;


            List<T> temp = new List<T>();

            var data = list.Where(entity =>
            {
                var p1 = ReflectHelper.GetValue(entity, "ParentId");
                return p1.IsEqual(parentId);
            });

            foreach (var item in data)
            {
                FindChildren(list, item);
                temp.Add(item);
            }
            return temp;
        }


        private static void FindChildren<T>(List<T> list, T entity)
        {

            var childrens = list.Where(t =>
            {
                var id = ReflectHelper.GetValue(entity, "Id");
                var parentId = ReflectHelper.GetValue(t, "ParentId");
                return id.IsEqual(parentId);
            }).ToList();

            if (childrens.Count > 0)
            {
                foreach (var child in childrens)
                {
                    FindChildren(list, child);
                }

                AddChildren(entity, childrens);
            }
        }

        private static void AddChildren<T>(T entity, List<T> childrens)
        {
            var listInstance = ReflectHelper.GetValue(entity, "Children") as List<T>;
            if (listInstance == null)
            {
                listInstance = new List<T>();
            }

            listInstance.AddRange(childrens);

            ReflectHelper.SetValue(entity, "Children", listInstance);
        }


    }
}

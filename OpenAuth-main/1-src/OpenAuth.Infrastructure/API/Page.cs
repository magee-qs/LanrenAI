using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    /// <summary>
    /// 分页数据
    /// </summary>
    public class Page
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int current { get; set; }

        /// <summary>
        /// 分页行数
        /// </summary>
        public int rows { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 排序列 
        /// </summary>
        public string sidx { get; set; }

        /// <summary>
        /// 排序方式 ASC DESC
        /// </summary>
        public string sord { get; set; }

        public List<Sorter> sorters { get; set; }

        public Page()
        {
            current = 1;
            rows = 10;
            total = 0;
            sorters = new List<Sorter>();
        }
    }


    /// <summary>
    /// 简单分页排序查询
    /// </summary>
    public class EasyPage
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 分页行数
        /// </summary>
        public int rows { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 排序列 Id, Name, Date
        /// </summary>
        public string sidx { get; set; }

        /// <summary>
        /// 排序方式 ASC DESC
        /// </summary>
        public string sord { get; set; }


        public EasyPage()
        {
            page = 1;
            rows = 10;
            total = 0;
            sidx = "Id";
            sord = "DESC";
        }
    }
     


    /// <summary>
    /// 排序
    /// </summary>
    public class Sorter
    {
        /// <summary>
        /// 排序列
        /// </summary>
        public string sidx { get; set; }

        /// <summary>
        /// 排序方式 ASC DESC
        /// </summary>
        public string sord { get; set; }

        public Sorter()
        {
            sidx = "Id";
            sord = "DESC";
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    /// <summary>
    /// excel操作工具类
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public static IEnumerable<T> IEnumerable<T>(string filePath, string sheetName) where T : class, new()
        {
            try
            {
                return MiniExcel.Query<T>(filePath, sheetName).ToList();
            }
            catch (Exception ex)
            {
                throw new CommonException("读取excel文件失败", 500, ex);
            }
        }

        /// <summary>
        /// 导出到excel下载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="CommonException"></exception>
        public static FileStreamResult Export<T>(IEnumerable<T> data, string fileName, MiniExcelLibs.IConfiguration configuration)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.SaveAs(data, configuration: configuration);
                    ms.Seek(0, SeekOrigin.Begin);

                    return new FileStreamResult(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    { FileDownloadName = fileName };
                }
            }
            catch (Exception ex)
            {
                throw new CommonException("导出到excel失败", 500, ex);
            }
        }
    }
}

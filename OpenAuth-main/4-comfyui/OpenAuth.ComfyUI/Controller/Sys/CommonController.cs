 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Controller.Sys
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class CommonController : ControllerBase
    {
        private string UploadPath()
        { 
            return Path.Combine("temp", DateTime.Now.ToString("yyyyMMdd")); 
        }

        /// <summary>
        /// 保存文件到默认路径wwwroot
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="CommonException"></exception>
        [HttpPost]
        public async Task<AjaxResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new CommonException("没有待上传文件", 500);


            if (FileHelper.IsSafeFile(file.FileName) == false)
                throw new CommonException("文件类型不正确",500);

            var mimeType = FileHelper.CheckFileType(file.FileName);

            var path = UploadPath();

            //随机文件名
            var fileName = FileHelper.GetRandomFileName(file.FileName, 64);

            //文件路径
            var filePath = Path.Combine(path, fileName);

            //物理目录
            var savePath =  Path.Combine(FileHelper.GetBasePath(), path);

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            //文件保存地址
            savePath = Path.Combine(savePath, fileName);

            long fileSize = 0;

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                fileSize = fileStream.Length;
                await file.CopyToAsync(fileStream);
            }

            FileDTO fileDTO = new FileDTO()
            {
                Id = IdGenerator.NextId().ToString(),
                FileName = file.FileName,
                FileSize = fileSize,
                FilePath = filePath,
                FileType = mimeType
            };

            return AjaxResult.OK(fileDTO, "");
        }


        /// <summary>
        /// 校验文件格式
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public AjaxResult CheckFileType([FromBody] string fileName)
        {
            var result = FileHelper.IsSafeFile(fileName);

            return AjaxResult.OK(result,"");
        }
    }

      
}

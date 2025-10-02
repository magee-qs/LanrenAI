using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAuth.Infrastructure.Helpers;

namespace OpenAuth.ComfyUI.WebAPI.Controllers
{
    [ApiController]
    [Route("/test")]
    public class TestController:ControllerBase
    {
        private string UploadPath()
        {
            return  FileHelper.Combine("temp", DateTime.Now.ToString("yyyyMMdd"));
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("/upload")]
        public AjaxResult Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new CommonException("没有待上传文件", 500);

            var path = UploadPath();

            //随机文件名
            var fileName = FileHelper.GetRandomFileName(file.FileName, 64);

            //文件路径
            var filePath = FileHelper.Combine(path, fileName);
             

            long fileSize = 0;


            using (var stream = file.OpenReadStream())
            {
                AliOssHelper.Upload(filePath, stream);
            }

            FileDTO fileDTO = new FileDTO()
            {
                Id = IdGenerator.NextId().ToString(),
                FileName = file.FileName,
                FileSize = fileSize,
                FilePath = filePath,
                FileType = MediaTypeHelper.GetMimeTypeByName(fileName)
            };

            return AjaxResult.OK(fileDTO, "");
        }
    }
}

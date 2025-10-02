using NLog;
using OpenAuth.Infrastructure.Helpers;
using Org.BouncyCastle.Utilities.Zlib;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth
{
    public class FileHelper
    {
       
        /// <summary>
        /// 从路径获取文件名
        /// D:\resources\images\1-xxx.jpg =>  D:\resources\images\ , 1-xxx.jpg
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileNameByPath(string filePath, out string path)
        {
            var index = filePath.LastIndexOf(@"\");
            path = filePath.Substring(0, index + 1);
            var fileName = filePath.Substring(index + 1);
            return fileName; 
        }

        /// <summary>
        /// 图片缩微图
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="thumbnailPath"></param>
        /// <returns></returns>
        public static string GetThumbnailName(string filePath, out string thumbnailPath)
        {
            var index = filePath.LastIndexOf(@"\");
            if (index == -1)
            {
                index = filePath.LastIndexOf("/");
            }
            thumbnailPath = filePath.Substring(0, index + 1);
            var fileName = filePath.Substring(index + 1);

            var newFileName = thumbnailPath + "thumb-" + fileName;
            return newFileName;
        }

        /// <summary>
        /// \resources\images\1-xxx.jpg =>  xxxx.jpg
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomFileName(string fileName, int length = 64)
        {
            var index = fileName.LastIndexOf(".");
            var name = CommonHelper.Random(length, true);
            return name + "." + fileName.Substring(index + 1);
        }


        /// <summary>
        /// \resources\images\1-xxx.jpg => .jpg
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileExt(string fileName)
        {
            var index = fileName.LastIndexOf(".");
            return fileName.Substring(index);
        }


        /// <summary>
        /// 图片缩微图
        /// </summary>
        /// <param name="inputImage"></param>
        /// <param name="outputImage"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static async Task CreateThumbnailImage(string inputImage, string outputImage, int width, int height)
        {
            using (var image = Image.Load(inputImage))
            {
                image.Mutate(t => t.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max   //保持图像比例
                }));

                await image.SaveAsync(outputImage);
            }
        }

         

        public static async Task<Stream> CreateThumbnailImage(Stream stream , int width, int height)
        {
            using (var image = Image.Load(stream))
            {
                image.Mutate(t => t.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max   //保持图像比例
                }));
                var outStream = new MemoryStream();
                // 如果没有指定格式，则保持原格式
                await image.SaveAsync(outStream, image.Metadata.DecodedImageFormat);
                // 重置流位置以便读取
                outStream.Position = 0;
                return outStream;
            }
        }


        /// <summary>
        /// 获取静态目录
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultBasePath()
        {
            var uploadConfig = ConfigHelper.AppSetting.UploadConfig;
            return Path.Combine(InternalApp.WebHostEnvironment.ContentRootPath, uploadConfig.Path);
        }
         
        public static string GetBasePath()
        {
            var uploadConfig = ConfigHelper.AppSetting.UploadConfig;
            return uploadConfig.UploadPath; 
        }

        /// <summary>
        /// 合并url路径
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var path in paths)
            {
                if (path.IsEmpty())
                    continue;

                if (path.StartsWith("/") == false)
                {
                    sb.Append("/");
                }
                sb.Append(path);
                
            }
            return sb.ToString();
        }



        private static  NLog.Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="dir"></param>
        /// <param name="_fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static async Task<FileDTO> SaveFile(HttpContent content, string dir, string _fileName, int width, int height)
        { 
            var uploadConfig = ConfigHelper.AppSetting.UploadConfig;
            logger.Info($"保存文件:{uploadConfig.UploadType}");
            if (uploadConfig.UploadType == "oss")
            { 
                return await SaveFileToOSS(content, dir, _fileName, width, height );
            }
            else
            {
                return await SaveFileToLocal(content, dir, _fileName, width, height);
            }
        }

        /// <summary>
        /// 保存图片并生成缩微图
        /// </summary>
        /// <param name="content"></param>
        /// <param name="dir"></param>
        /// <param name="_fileName"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static async Task<FileDTO> SaveFileToOSS(HttpContent content, string dir ,string _fileName, int width, int height)
        {
            logger.Info($"保存文件: dir={dir}, fileName = {_fileName}, scaleWidth={width}, scaleHeight=height");

            //文件重命名 
            var fileName =  GetRandomFileName(_fileName);

            //定义保存路径
            var filePath =  Combine(dir, DateTime.Now.ToString("yyyyMMdd"), fileName);
             

            //获取文件流
            var stream = content.ReadAsStream();

            long fileSize = stream.Length;
             
            logger.Info($"保存大图:fileName = {fileName}, fileSize = {fileSize}");

            
            //上传文件
            AliOssHelper.Upload(filePath, stream);

            //上传缩微图
            //缩微图 
            var thumbName = GetThumbnailName(filePath, out var thumbPath);


            stream.Position = 0;

            var thumbSteram = await CreateThumbnailImage(stream, width, height);

            var thumb_fileSize = thumbSteram.Length;

           
            logger.Info($"保存小图 : fileName = {thumbName}, fileSize = {thumb_fileSize}");

            //上传缩微图
            AliOssHelper.Upload(thumbName, thumbSteram);

            return new FileDTO()
            {
                FileName = fileName,
                FilePath = filePath,
                FileType = MediaTypeHelper.GetMimeTypeByName(fileName),
                FileSize = fileSize
            };
        }



        public static async Task<FileDTO> SaveFileToLocal(HttpContent content, string dir, string _fileName, int width, int height)
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            logger.Info($"保存文件: dir={dir}, fileName = {_fileName}, scaleWidth={width}, scaleHeight=height"); 

            var uploadPath = Path.Combine(GetBasePath(), dir, date);
            logger.Info("路径:" + uploadPath);
            //判断目录是否存在
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            //保存地址
            string savePath = Path.Combine(uploadPath, _fileName);
            logger.Info("物理地址:" + uploadPath);

            //相对地址 返回数据
            string filePath = Path.Combine(dir, date, _fileName);
            logger.Debug("相对路径:" + filePath);

            var buffer = await content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync(savePath, buffer);


            //缩微图 
            var fileName = FileHelper.GetThumbnailName(savePath, out var thumbPath);
            logger.Info("开始生成缩微图:" + fileName);
            await FileHelper.CreateThumbnailImage(savePath, fileName, 205, 360);

            return new FileDTO()
            {
                FileName = _fileName,
                FilePath = filePath,
                FileType = MediaTypeHelper.GetMimeTypeByName(_fileName),
                FileSize = buffer.Length
            };

        }


        public static string CheckFileType(string fileName)
        {
            var mimeType = MediaTypeHelper.GetStrictMimeTypeByName(fileName);
            if (mimeType == null)
                throw new CommonException("文件类型不正确", 500);

            return mimeType;
        }


        public static bool IsSafeFile(string fileName)
        {
            if(fileName.IsEmpty())
                throw new CommonException("文件类型不正确", 500); 

            var fileExt = GetFileExt(fileName);
            if (fileExt.IsEmpty())
                throw new CommonException("文件类型不正确", 500);

            if (_excludefiles.Contains(fileExt))
                throw new CommonException("文件类型不正确", 500);

            return true;
        }

        private static readonly string[] _excludefiles = { 
            ".exe", ".bat", ".cmd", ".ps1", ".sh", ".jar", ".msi",
            ".js", ".vbs", ".php", ".py", ".pl", ".rb",
            ".html", ".htm", ".asp", ".aspx", ".jsp", ".cer",
            ".config", ".htaccess", ".htpasswd",
            ".dll", ".com", ".scr", ".pif", ".reg", ".cpl", ".wsf" };
    }
}

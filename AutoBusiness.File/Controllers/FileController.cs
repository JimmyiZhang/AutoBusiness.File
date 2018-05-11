using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using AutoBusiness.Library.Web;
using AutoBusiness.Library;

namespace AutoBusiness.File.Controllers
{
    public class FileController : ApiController
    {
        #region Private
        private string root;
        private string Root
        {
            get
            {
                if (string.IsNullOrEmpty(this.root)) this.root = HttpContext.Current.Server.MapPath("~/Files");
                return this.root;
            }
        }

        private string GetRealName(string filePath)
        {
            var extendName = System.IO.Path.GetExtension(filePath);
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string fullFolder = Path.Combine(Root, dateFolder);

            if (!Directory.Exists(fullFolder))
            {
                Directory.CreateDirectory(fullFolder);
            }

            var realName = string.Format("{0}/{1}{2}", dateFolder, Guid.NewGuid().ToString(), extendName);
            return realName;
        }
        private string GetFullName(string filePath)
        {
            return Path.Combine(Root, filePath);
        }
        private string GetThumbnailName(string filePath)
        {
            var extendName = System.IO.Path.GetExtension(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var directoryName = Path.GetDirectoryName(filePath);

            return string.Format("{0}/{1}_t{2}", directoryName, fileName, extendName);
        }
        #endregion

        [HttpOptions]
        public void Option()
        {
        }

        // 上传图片
        [HttpPost]
        public IEnumerable<UploadResult> UploadFile()
        {
            var results = new List<UploadResult>();

            HttpRequest req = HttpContext.Current.Request;

            // base64方式, 则解码并保存
            var type = HttpContext.Current.Request.Params["type"];
            if (type.IsNotEmpty() && type == "base64")
            {
                UploadImage image = new UploadImage()
                {
                    image = req.Params["image"],
                    tag = req.Params["tag"]
                };

                var result = this.UploadApp(image);
                results.Add(result);
                return results;
            }

            // 其他格式
            for (int i = 0; i < req.Files.Count; i++)
            {
                var file = req.Files[i];
                // origin file
                var realName = this.GetRealName(file.FileName);
                var realPath = this.GetFullName(realName);
                ImageHelper.Thumbnail(file.InputStream, realPath, 1000, 1000, 75);
                // thunbnail file
                var thumName = this.GetThumbnailName(realName);
                var thumPath = this.GetFullName(thumName);
                ImageHelper.Thumbnail(file.InputStream, thumPath, 60, 60, 72);
                // dispose
                file.InputStream.Close();
                // return result
                results.Add(
                    new UploadResult()
                    {
                        name = file.FileName,
                        size = file.ContentLength,
                        type = file.ContentType,
                        origin = "/files/" + realName,
                        thumbnaill = "/files/" + thumName,
                    });
            }

            // 参数含redirect, 则是跨域上传, 直接redirect
            var redirect = req.Params["redirect"];
            if (redirect.IsNotEmpty())
            {
                var json = JsonConvert.SerializeObject(results);
                HttpContext.Current.Response.Redirect(redirect + HttpUtility.UrlEncode(json));
                HttpContext.Current.Response.End();
            }

            // 参数不含则直接返回
            return results;
        }

        // Base64 上传图片
        [HttpPost]
        public UploadResult UploadApp(UploadImage image)
        {
            string fileBase64 = image.image;

            // 查询头部信息, 确定文件格式, 基本格式如: data:image/png;base64,"
            Regex reg = new Regex("^data:image/(?<t>[a-z]+);base64,");
            Match match = reg.Match(fileBase64);

            string fileType = "png";
            if (match.Success)
            {
                fileType = match.Groups["t"].Value;
                int fileHead = match.Value.Length;
                fileBase64 = fileBase64.Substring(fileHead);
            }

            // stream
            byte[] pic = Convert.FromBase64String(fileBase64);
            MemoryStream ms = new MemoryStream(pic);

            // origin file
            var realName = this.GetRealName("app." + fileType);
            var realPath = this.GetFullName(realName);
            ImageHelper.Thumbnail(ms, realPath, 1000, 1000, 75);

            // thumbnail file
            var thumName = this.GetThumbnailName(realName);
            var thumPath = this.GetFullName(thumName);
            ImageHelper.Thumbnail(ms, thumPath, 60, 60, 72);

            // dispose
            ms.Close();

            return new UploadResult()
            {
                name = realName,
                type = fileType,
                origin = "/files/" + realName,
                thumbnaill = "/files/" + thumName,
                tag = image.tag
            };
        }
    }
}
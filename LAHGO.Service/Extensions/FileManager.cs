using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Service.Extensions
{
    public static class FileManager
    {
        //public static bool CheckFileSize(this IFormFile file, double length)
        //{
        //    return ((double)file.Length / 1024 > length);
        //}
        //public static bool CheckFileType(this IFormFile file, string type)
        //{
        //    return file.ContentType == type;
        //}
        public async static Task<string> CreateAsync(this IFormFile file, IWebHostEnvironment env, params string[] folders)
        {
            string filename = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + file.FileName;
            string path = env.WebRootPath;
            foreach (string folder in folders)
            {
                path = Path.Combine(path, folder);
            }
            path = Path.Combine(path, filename);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filename;
        }
    }
}

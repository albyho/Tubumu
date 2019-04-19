using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Tubumu.Modules.Admin.Models.Input
{
    public class UserImageInput
    {
        [Required(ErrorMessage = "请选择用户")]
        [Range(1, Int32.MaxValue, ErrorMessage = "请选择合法的用户")]
        public int UserId {get;set;}

        //[Required(ErrorMessage = "请选择图片")]
        //[FileExtensions(Extensions = ".jpg,.png", ErrorMessage = "图片格式错误(仅支持 jpg 或 png)")]
        //public IFormFile File { get; set; }

        public IFormCollection FileCollection {get;set;}
    }
}

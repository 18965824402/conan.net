﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace conan.Saas.Web.Model
{
    /// <summary>
    /// 上传图片
    /// </summary>
    public class uploadfile
    {
      //  [Required]
	
     //   [Display(Name = "身份证附件")]
	
      // [FileExtensions(Extensions = ".jpg", ErrorMessage = "图片格式错误")]
	
        public IFormFile ShareImg { get; set; }
    }

  
}

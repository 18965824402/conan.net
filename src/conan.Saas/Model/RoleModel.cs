﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using cloudscribe.Web.Pagination;

namespace conan.Saas.Model
{

    public class BaseListViewModel<T>
    {
        public BaseListViewModel()
        {
            Paging = new PaginationSettings();
        }

        public string Query { get; set; } = string.Empty;

        public List<T> list { get; set; } = null;

        public PaginationSettings Paging { get; set; }
    }
     
    public  class Rolemodel :GoBackUrlModel
    {
        public long Id { get; set; }


        [Display(Name = "角色名称")]
        [Required(ErrorMessage = "{0}必填")]
        public string Name { get; set; }



        public bool AllowEdit { get; set; } = false;

        public bool AllowDelete { get; set; } = false;

        [Display(Name = "显示顺序")]
        [Required(ErrorMessage = "{0}必填")]
        public int? SortCode { get; set; } = 0;


        [Display(Name = "备注")]
     
        [StringLength(100, ErrorMessage = "{0}最大100位字符")]

        public string Description { get; set; }

        public List<long> PermissionIds { get; set; }
    }




  


}

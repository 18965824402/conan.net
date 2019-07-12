﻿/**************************************************************************
 * 作者：X   
 * 日期：2017.01.18   
 * 描述：
 * 修改记录：    
 * ***********************************************************************/
using dotNET.Dto;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotNET.Domain.Entities
{
    [Table("Modules")]
    public class Module : Entity, IEntity
    {
        public long ParentId { get; set; }
        public string EnCode { get; set; }
        public string FullName { get; set; }
        public string Icon { get; set; }
        public string UrlAddress { get; set; }
        public string Target { get; set; }
        public bool? IsMenu { get; set; }
        public bool? IsExpand { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? AllowEdit { get; set; }
        public bool? AllowDelete { get; set; }
        public int? SortCode { get; set; }
        public string Description { get; set; }
        public DateTime CreatorTime { get; set; }
      

        /// <summary>
        /// 生成ID
        /// </summary>
        /// <returns></returns>
        public long CreateId()
        {
            return base.CreateId(EntityEnum.Module);
        }
    }
}

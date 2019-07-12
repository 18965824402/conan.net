﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNET.Dto
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class LoginLogOption : Option
    {
        public string LoginId { get; set; }

        /// <summary>
        /// 登录类型 Saas / Agent /Member
        /// </summary>
        public string LoginType { get; set; }

        /// <summary>
        /// 所属代理
        /// </summary>
        public string AgentId { get; set; }

      

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? kCreatorTime { get; set; }



        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime? eCreatorTime { get; set; }
    }
}

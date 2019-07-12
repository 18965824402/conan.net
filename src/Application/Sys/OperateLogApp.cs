﻿using System;
using System.Collections.Generic;
using System.Text;
using dotNET.Application.Infrastructure;
using System.Threading.Tasks;
using dotNET.Core;
using dotNET.Domain.Entities;
using dotNET.Dto;
using System.Linq;
using System.ComponentModel;
using dotNET.EFCoreRepository;
using Microsoft.EntityFrameworkCore;

namespace dotNET.Application.App
{
    public class OperateLogApp : IAppService, IOperateLogApp
    {
        #region 注入
        public IBaseRepository<OperateLog> OperateLogRep { get; set; }
        public IBaseRepository<User> UserRep { get; set; }
        #endregion

        /// <summary>
        /// 自定义 日志内容
        /// </summary>
        /// <param name="uinfo"></param>
        /// <param name="tag"></param>
        /// <param name="content"></param>
        public async Task CustomLogAsync(CurrentUser curUser, string tag, string content)
        {
            OperateLog log = new OperateLog
            {
                Tag = tag,
                Content = content,
                Operator = curUser.RealName,
                OperatorId = curUser.Id,
                IP = curUser.LoginIPAddress
            };
            log.Id = log.CreateId();
            if (!string.IsNullOrWhiteSpace(content))
                await OperateLogRep.AddAsync(log);
        }

        /// <summary>
        /// 对象添加 的日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user"></param>
        /// <param name="tag"></param>
        /// <param name="t"></param>
        /// <param name="ip"></param>
        public async Task InsertLogAsync<T>(CurrentUser curUser, string tag, T t) where T : class, new()
        {
            OperateLog log = new OperateLog
            {
                Tag = tag,
                Content = ObjectToStr(t, "UpdatePerson,UpdateDate"),
                Operator = curUser.RealName,
                OperatorId = curUser.Id,
                IP = curUser.LoginIPAddress
            };
            log.Id = log.CreateId();
            await OperateLogRep.AddAsync(log);
        }

        /// <summary>
        /// 对象删除 的日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user"></param>
        /// <param name="tag"></param>
        /// <param name="t"></param>
        /// <param name="ip"></param>
        public async Task RemoveLogAsync<T>(CurrentUser curUser, string tag, T t) where T : class, new()
        {
            OperateLog log = new OperateLog
            {
                Tag = tag,
                Content = ObjectToStr(t),
                Operator = curUser.RealName,
                OperatorId = curUser.Id,
                IP = curUser.LoginIPAddress
            };
            await OperateLogRep.AddAsync(log);
        }

        /// <summary>
        /// 对象编辑 的日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user"></param>
        /// <param name="tag"></param>
        /// <param name="ip"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        public async Task EditLogAsync<T>(CurrentUser curUser, string tag, T before, T after) where T : class, new()
        {
            OperateLog log = new OperateLog
            {
                Tag = tag,
                Content = ObjectEquals(before, after, "CreatePerson,CreateDate,UpdatePerson,UpdateDate"),
                Operator = curUser.RealName,
                OperatorId = curUser.Id,
                IP = curUser.LoginIPAddress
            };
            if (!string.IsNullOrEmpty(log.Content))
                await OperateLogRep.AddAsync(log);
        }


        /// <summary>
        /// 获取对象的修改值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        private string ObjectEquals<T>(T t1, T t2, string excepts = "") where T : class, new()
        {
            string[] arrs = excepts.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            Type p1 = t1.GetType();
            Type p2 = t2.GetType();
            var ns = p1.GetProperties();
            foreach (var p in ns)
            {
                string name = p.Name;
                if (arrs.Contains(name))
                    continue;
                string cnname = ((DescriptionAttribute)Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute)))?.Description;
                var v1 = p1.GetProperty(name).GetValue(t1, null);
                var v2 = p2.GetProperty(name).GetValue(t2, null);
                //Id处理
                if (name.ToString().ToLower() == "id")
                {
                    if (sb.Length > 0 && v1.Equals(v2))
                    {
                        sb.AppendFormat($"{cnname} 值为 \"{v1}\" \r\n", cnname ?? p.Name, v1 ?? "");
                    }
                    continue;
                }
                #region 判断
                if (v1 == null && v2 == null)
                {
                    continue;
                }
                else
                          if (v1 == null && v2 != null)
                {
                    sb.AppendFormat($"修改了 {cnname} 旧值为 \"{v1}\",新值为 \"{v2}\"\r\n", cnname ?? name, v1 ?? "", v2 ?? "");

                }
                else
                       if (v1 != null && v2 == null)
                {
                    sb.AppendFormat($"修改了 {cnname} 旧值为 \"{v1}\",新值为 \"{v2}\"\r\n", cnname ?? name, v1 ?? "", v2 ?? "");

                }
                else
                       if (v1 != null && v2 != null)
                {
                    if (v1.Equals(v2))
                        continue;
                    else
                        sb.AppendFormat($"修改了 {cnname} 旧值为 \"{v1}\",新值为 \"{v2}\"\r\n", cnname ?? name, v1 ?? "", v2 ?? "");

                }
                #endregion
            }
            return sb.ToString();
        }

        /// <summary>
        /// 对象转成字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="excepts"></param>
        /// <returns></returns>
        private string ObjectToStr<T>(T t, string excepts = "") where T : class, new()
        {
            string[] arrs = excepts.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder();
            Type p1 = t.GetType();
            foreach (var p in p1.GetProperties())
            {
                if (arrs.Contains(p.Name))
                    continue;
                string cnname = ((DescriptionAttribute)Attribute.GetCustomAttribute(p, typeof(DescriptionAttribute)))?.Description;

                object v = p.GetValue(t);
                sb.AppendFormat($"{cnname} 值为 \"{v}\" \r\n", cnname ?? p.Name, v ?? "");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 对象复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RealObject"></param>
        /// <returns></returns>
        public T Clone<T>(T RealObject) where T : class, new()
        {
            string s = JsonHelper.SerializeObject(RealObject);
            return JsonHelper.DeserializeObject<T>(s);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task<Page<OperateLogDto>> PagerAsync(OperateLogOption option)
        {
            var predicate = PredicateBuilder.True<OperateLog>();
            if (option.StartDateTime != null && option.StartDateTime.HasValue)
            {
                predicate = predicate.And(o => o.CreateTime >= option.StartDateTime.Value);
            }
            if (option.EndDateTime != null && option.EndDateTime.HasValue)
            {
                predicate = predicate.And(o => o.CreateTime <= option.EndDateTime.Value);
            }
            if (!string.IsNullOrWhiteSpace(option.Tag))
            {
                predicate = predicate.And(o => o.Tag == option.Tag);
            }
            List<OperateLogDto> data = new List<OperateLogDto>();
            int total = await OperateLogRep.GetCountAsync(predicate);
            if (total > 0)
            {
                IEnumerable<OperateLog> result = (await OperateLogRep.Find(option.PageIndex, option.Limit, option.OrderBy, predicate).ToListAsync());
                data = MapperHelper.MapList<OperateLog, OperateLogDto>(result.ToList());
                if (data != null && data.Count > 0)
                {
                    var list = data.Select(o => o.OperatorId).ToList();
                    var userlist = await UserRep.Find(o => list.Contains(o.Id)).ToListAsync();
                    int i = 0;
                    foreach (var item in data)
                    {
                        long oid = item.OperatorId;
                        var uname = userlist.Where(o => o.Id == oid).FirstOrDefault();
                        data[i].OperatorName = uname?.RealName ?? "";
                        i++;
                    }
                }
            }
            return new Page<OperateLogDto>(total, data);
        }

        public async Task<PageResult<OperateLogDto>> GetPageAsync(int pageNumber, int rowsPrePage, OperateLogOption option)
        {
            List<OperateLogDto> data = new List<OperateLogDto>();
            PageResult<OperateLogDto> list = new PageResult<OperateLogDto>();
            string orderby = " id desc";
            var predicate = PredicateBuilder.True<OperateLog>();

            if (option.StartDateTime != null && option.StartDateTime.HasValue)
            {
                predicate = predicate.And(o => o.CreateTime >= option.StartDateTime.Value);

            }
            if (option.EndDateTime != null && option.EndDateTime.HasValue)
            {
                predicate = predicate.And(o => o.CreateTime <= option.EndDateTime.Value);
            }
            if (!string.IsNullOrWhiteSpace(option.Tag))
            {

                predicate = predicate.And(o => o.Tag == option.Tag);

            }
            var tlist = await OperateLogRep.Find(pageNumber, rowsPrePage, orderby, predicate).ToListAsync();
            data = MapperHelper.MapList<OperateLog, OperateLogDto>(tlist.ToList());
            if (data != null && data.Count > 0)
            {
                var OperatorIds = data.Select(o => o.OperatorId).ToList();
                var userlist = await UserRep.Find(o => OperatorIds.Contains(o.Id)).ToListAsync();
                int i = 0;
                foreach (var item in data)
                {
                    long oid = item.OperatorId;
                    var uname = userlist.Where(o => o.Id == oid).FirstOrDefault();
                    data[i].OperatorName = uname?.RealName ?? "";
                    i++;
                }
            }
            list.Data = data.ToList();
            int total = await OperateLogRep.GetCountAsync(predicate);
            list.ItemCount = total;
            return list;
        }
    }
}
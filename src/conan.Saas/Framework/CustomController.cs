using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using dotNET.Core;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace conan.Saas.Framework
{
    public class CustomController : Controller
    {
        //��ҳ����
        public int DefaultPageSize = 5;
        /// <summary>
        /// ��ȡModelState�еĴ�����Ϣ�����ֵ伯�ϵ���ʽ����
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetErrorFromModelState()
        {
            var errors = new Dictionary<string, string>();
            foreach (var key in ModelState.Keys)
            {
                if (ModelState[key].Errors.Count > 0)
                {
                    errors[key] = ModelState[key].Errors[0].ErrorMessage;
                }
            }
            return errors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetErrorFromModelStateStr()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            var errors = new Dictionary<string, string>();
            foreach (var key in ModelState.Keys)
            {
                if (ModelState[key].Errors.Count > 0)
                {
                    sb.AppendFormat($"{key}:{ModelState[key].Errors[0].ErrorMessage}");
                }
            }
            return sb.ToString().TrimEnd(',');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override JsonResult Json(object data)
        {
            var setting = new Newtonsoft.Json.JsonSerializerSettings();
            setting.Converters.Add(new HexLongConverter());
            setting.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            setting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            setting.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            return base.Json(data, setting);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult NotFind()
        {
            return View();
        }

        /// <summary>
        /// ���ز����ɹ�/ʧ����ʾ
        /// </summary>
        /// <param name="IsSucceeded"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        protected JsonResult Operation(bool IsSucceeded, string Message, string GoBackUrl = "")
        {
            return Json(new { GoBackUrl = GoBackUrl, IsSucceeded = IsSucceeded, Message = Message });
        }


        /// <summary>
        /// ���� Bootstrap Table ����ҳ json ����
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        protected JsonResult List(dynamic list)
        {
            return Json(new { rows = list });
        }



        /// <summary>
        /// ��ȡ��ǰ�û�
        /// </summary>
        /// <returns></returns>
        public CurrentUser CurrentUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return null;
                }
                string userdata = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.UserData).Value;
                return CurrentUser.FromJson(userdata);
            }
        }


        /// <summary>
        /// ��ǰ��url     string url = GetRedirectUrl(this.HttpContext.Request);
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetRedirectUrl(HttpRequest request)
        {
            var builder = new UriBuilder()
            {
                Path = request.Path,
                Query = request.QueryString.ToUriComponent()
            };

            return builder.Uri.PathAndQuery;
        }


        public string SetingBackUrl(HttpRequest request)
        {

            string goBackUrl = request.HttpContext.Request.Query["GoBackUrl"].ToString();
            if (string.IsNullOrWhiteSpace(goBackUrl))
            {
                var urlRef = request.HttpContext.Request.Headers["Referer"].FirstOrDefault();
                if (urlRef != null)
                    goBackUrl = urlRef.ToString();
            }

            return goBackUrl;
        }


        public ActionResult RedirectToUrl(string url, string actionName = "index")
        {
            if (!string.IsNullOrWhiteSpace(url))
                return Redirect(url);
            else
                return RedirectToAction(actionName);
        }



    }
}
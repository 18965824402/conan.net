﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Diagnostics;

namespace conan.Saas.Framework
{
  /// <summary>
  /// 页面的执行时间
  /// </summary>
    public class ExecuteTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public ExecuteTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var body = httpContext.Response.Body;

            var ms = new MemoryStream();
            httpContext.Response.Body = ms;

            Stopwatch sw = new Stopwatch();

            try
            {
                sw.Start();
                await _next.Invoke(httpContext);
                sw.Stop();
                httpContext.Response.Headers["ExecuteTime"] = sw.ElapsedMilliseconds.ToString();
                dotNET.Core.NLogger.Info($"RequestUrl:{httpContext.Request.Path}, ExecuteTime:{sw.ElapsedMilliseconds}");
                ms.Position = 0;
                await ms.CopyToAsync(body);
            }
            finally
            {
                httpContext.Response.Body = body;
            }
        }
    }
}

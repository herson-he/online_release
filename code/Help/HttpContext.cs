using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Help
{
    /// <summary>
    /// 请求上下文帮助类
    /// </summary>
    public static class HttpContext
    {
        // public static IServiceCollection serviceCollection;

        /// <summary>
        /// 当前请求上下文
        /// </summary>
        //public static Microsoft.AspNetCore.Http.HttpContext Current
        //{
        //    get
        //    {
        //        object factory = serviceCollection.BuildServiceProvider().GetService(typeof(IHttpContextAccessor));
        //        Microsoft.AspNetCore.Http.HttpContext context = ((HttpContextAccessor)factory).HttpContext;
        //        return context;
        //    }
        //}

        //public static Microsoft.AspNetCore.Http.HttpContext Current
        //{
        //    get; set;
        //}


        private static IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// 当前上下文
        /// </summary>
        public static Microsoft.AspNetCore.Http.HttpContext Current => _contextAccessor.HttpContext;

        public static void Configure(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
    }
}

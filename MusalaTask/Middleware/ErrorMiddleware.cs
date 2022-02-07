using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(httpContext, exception);
            }
        }

        private Task HandleErrorAsync(HttpContext context, Exception exception)
        {
            int? sqlNumber = null;
            var response = new { message = exception.Message };
            var payload = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var test = exception.StackTrace;

            var errorList = new List<string>();
            if (exception.InnerException != null)
            {

                errorList.Add(exception.InnerException.Message);
                var sqlEx = exception as SqlException;
                if (sqlEx != null)
                {
                    var num = ((SqlException)exception.InnerException).Number;
                    sqlNumber = num;
                    errorList.Add(num + "");
                }

            }
            else
                errorList.Add(exception.Message);


            if (context.User != null && context.User.Claims != null && context.User.Claims.Count() > 0)
            {
                var userId = context.User.Claims.FirstOrDefault().Value;
                int user_id = 0;
                if (int.TryParse(userId, out int id))
                {
                    user_id = id;
                }

                //exceptionLogService.createExceptionLog(user_id, context.Request.Path.Value, exception, sqlNumber);

            }


            return context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                code = 500,
                error = errorList
            }));
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorMiddleware>();
        }
    }
}

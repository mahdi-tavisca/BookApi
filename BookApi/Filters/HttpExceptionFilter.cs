using BookApi.Contracts;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookApi.Filters
{
    public class HttpExceptionFilter : Attribute, IExceptionFilter
    {
        private LogsService _logsService;
        public async void OnException(ExceptionContext context)
        {
            _logsService = (LogsService)context.HttpContext.RequestServices.GetService(typeof(LogsService));
            var errorResponse = new { error = context.Exception.Message };
            context.Result = new JsonResult(errorResponse);
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
           
            var details = JsonSerializer.Serialize(new {
                errorResponse,
                context.HttpContext.Response.StatusCode
            });

            var log = new Log
            {
                Type = "Exception",
                Details = details
            };
            await _logsService.CreateLog(log);
        }
    }
}

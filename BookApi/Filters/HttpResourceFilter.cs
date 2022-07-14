using BookApi.Contracts;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Filters
{
    public class HttpResourceFilter : Attribute, IResourceFilter
    {
        private LogsService _logsService;
        public HttpResourceFilter()
        {
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {

        }

        public async void OnResourceExecuting(ResourceExecutingContext context)
        {
            _logsService = (LogsService)context.HttpContext.RequestServices.GetService(typeof(LogsService));
            var path = context.HttpContext.Request.Path;

            var log = new Log
            {
                Type = "Resource filter path log",
                Details = path
            };
            await _logsService.CreateLog(log);
        }
    }
}

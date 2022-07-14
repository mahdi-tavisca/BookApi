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
    public class HttpAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private const string USER_ROLE = "userRole";
        private LogsService _logsService;
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            _logsService = (LogsService)context.HttpContext.RequestServices.GetService(typeof(LogsService));
            var errorLogType = "Authorization Error";
            if (context != null)
            {
                context.HttpContext.Request.Headers.TryGetValue("userRole", out var authValue);
                if (string.IsNullOrEmpty(authValue))
                {
                    var log = new Log
                    {
                        Type = errorLogType,
                        Details = $"{USER_ROLE} is required"
                    };
                    await _logsService.CreateLog(log);
                    context.Result = new UnauthorizedObjectResult($"{USER_ROLE} is required");
                    return;
                }
                if (!authValue.Equals("admin"))
                {
                    var log = new Log
                    {
                        Type = errorLogType,
                        Details = $"{USER_ROLE} is invalid"
                    };
                    await _logsService.CreateLog(log);
                    context.Result = new UnauthorizedObjectResult($"{USER_ROLE} is invalid");
                    return;
                }
            }
        }
    }
};

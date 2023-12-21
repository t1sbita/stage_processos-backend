using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace StageProcessos.Presentation.Filters;

public class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
{

    /// <summary>
    /// ExceptionHandlerFilterAttribute.OnException
    /// </summary>
    /// <param name="context"></param>
    public override void OnException(ExceptionContext context)
    {
        if (context != null)
        {
            context.HttpContext.Response.ContentType = "application/problem+json";

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.Result = new JsonResult(new
            {
                status = context.HttpContext.Response.StatusCode,
                error = context.Exception.Message,
                stackTrace = context.Exception.StackTrace
            });

        }
    }
}
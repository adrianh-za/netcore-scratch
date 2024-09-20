using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MixedAPIs.Exceptions;

public class ExceptionHandlingAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        //var message = context.Exception.Message;
        context.Result = new OkObjectResult("FROM ExceptionHandlingAttribute");
    }
}
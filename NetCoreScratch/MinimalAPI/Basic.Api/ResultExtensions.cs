using System.Net.Mime;
using System.Text;

namespace Basic.Api;

public static class ResultExtensions
{
    public static IResult Html(this IResultExtensions extensions, string html)
    {
        return new HtmlResult(html);
    }
}

public class HtmlResult(string html) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(html);
        return httpContext.Response.WriteAsync(html);
    }
}
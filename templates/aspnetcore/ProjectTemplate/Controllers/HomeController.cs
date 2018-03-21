using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Vostok.Tracing;

namespace ProjectTemplate.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("{*url}")]
        public object Echo()
        {
            return Json(new
            {
                url = Request.GetUri(),
                traceUrl = $"http://localhost:6301/{TraceContext.Current.TraceId}",
                traceId = TraceContext.Current.TraceId
            });
        }
    }
}

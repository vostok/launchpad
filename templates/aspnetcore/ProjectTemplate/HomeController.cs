using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ProjectTemplate
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("{*url}")]
        public object Echo()
        {
            return Json(new {url = Request.GetUri()});
        }
    }
}

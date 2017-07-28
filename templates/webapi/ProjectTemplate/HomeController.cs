using System.Web.Http;

namespace ProjectTemplate
{
    [RoutePrefix("")]
    public class HomeController : ApiController
    {
        [Route("{*url}")]
        [HttpGet]
        public object Echo()
        {
            return Json(new { url = Request.RequestUri });
        }
    }
}
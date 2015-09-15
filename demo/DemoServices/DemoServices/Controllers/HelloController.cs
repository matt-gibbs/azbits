
using System;
using System.Web.Http;

namespace DemoServices.Controllers
{
    public class HelloController : ApiController
    {
        // GET api/Hello
        public string Get()
        {
            return $"Hello @ {DateTime.UtcNow}";
        }
    }
}
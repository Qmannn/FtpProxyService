using System.Web.Mvc;
using Newtonsoft.Json;

namespace Proxynet.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public new ContentResult Json( object value )
        {
            return Content( JsonConvert.SerializeObject( value ), "application/json" );
        }
    }
}
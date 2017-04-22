using System.Web.Mvc;

namespace Proxynet.Controllers
{
    public class PagesController : BaseController
    {
        // GET: Pages
        public ActionResult Users()
        {
            return PartialView( "Users" );
        }

        public ActionResult UserEdit()
        {
            return PartialView("User");
        }

        // GET: Pages
        public ActionResult TestPage()
        {
            return PartialView( "User" );
        }
    }
}
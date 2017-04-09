using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsersLib.DbControllers;

namespace Proxynet.Controllers
{
    public class UserController : BaseController
    {
        private readonly IDbUserController _dbUserController;
        private readonly IDbSiteController _dbSiteController;

        public UserController( IDbUserController dbUserController, IDbSiteController dbSiteController )
        {
            _dbUserController = dbUserController;
            _dbSiteController = dbSiteController;
        }

        public ActionResult Index()
        {
            ViewBag.Users = _dbUserController.GetUsers();
            return View();
        }

        public ActionResult Edit( int userId )
        {
            return View();
        }
    }
}
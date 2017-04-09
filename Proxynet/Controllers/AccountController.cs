using Proxynet.Models;
using Proxynet.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;

namespace Proxynet.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager _auth;
        private readonly IAccount _accountServices;

        public AccountController( IAuthenticationManager auth, IAccount accountServices )
        {
            _auth = auth;
            _accountServices = accountServices;
        }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login( string returnUrl )
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login( LoginViewModel model )
        {
            if( !ModelState.IsValid )
                return View();

            if( _accountServices.getUsers().Any( a => a.userName == model.UserName && a.password == model.Password && a.status ) )
            {
                var identity = new ClaimsIdentity( new[] { new Claim( ClaimTypes.Name, model.UserName ), }, DefaultAuthenticationTypes.ApplicationCookie );

                _auth.SignIn( new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                }, identity );

                return RedirectToAction( "Index", "User" );
            }
            ModelState.AddModelError( "", "Invalid login attempt." );
            return View( model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _auth.SignOut();
            return RedirectToAction( "Login", "Account" );
        }

    }
}
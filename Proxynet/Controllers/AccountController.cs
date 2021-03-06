﻿using System;
using Proxynet.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Web.Mvc;
using UsersLib.Service.Auth;

namespace Proxynet.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager _auth;
        private readonly IAuthorizer _ldapAuthorizer;

        public AccountController( IAuthenticationManager auth, IAuthorizer ldapAuthorizer )
        {
            _auth = auth;
            _ldapAuthorizer = ldapAuthorizer;
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

            if ( _ldapAuthorizer.ValidateCredentials( model.UserName, model.Password ) )
            {
                var identity = new ClaimsIdentity( new[] { new Claim( ClaimTypes.Name, model.UserName ), },
                    DefaultAuthenticationTypes.ApplicationCookie );

                _auth.SignIn( new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                }, identity );

                if (String.IsNullOrEmpty(ViewBag.ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(ViewBag.ReturnUrl);
            }
            ModelState.AddModelError( "", @"Неверный логин или пароль." );
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
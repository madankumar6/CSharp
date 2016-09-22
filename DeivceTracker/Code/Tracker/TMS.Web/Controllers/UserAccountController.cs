using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.BusinessRule.Interfaces;
using TMS.Web.Models.ViewModels;
using TMS.Model;
using TMS.Web.Rules;
using Newtonsoft.Json;
using TMS.BusinessRule.Concretes;
using System.Web.Security;
using TMS.DAL;
using System.Data.Entity;
using System.Security.Principal;
using System.Net;
using Utils;

namespace TMS.Web.Controllers
{
    public class UserAccountController : Controller
    {
        #region Properties
        private static readonly string FileName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType?.ToString();
        internal static Log4NetWrap Logger = new Log4NetWrap(FileName);

        private readonly IUserService<User> _userService;
        #endregion

        public UserAccountController(IUserService<User> userService)
        {
            this._userService = userService;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: /UserAccount/Login
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /UserAccount/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = _userService.ValidateUser(model.Username, model.Password);

                    if (user == null)
                    {
                        ModelState.AddModelError("", "Username is not available");
                        return View(model);
                    }

                    int userRoleId = 1;
                    string redirectToController = "Home";

                    if (user is Admin)
                    {
                        userRoleId = 1;
                        redirectToController = "Admin";
                    }
                    else if (user is Distributor)
                    {
                        userRoleId = 2;
                        redirectToController = "Distributor";
                    }
                    else if (user is Dealer)
                    {
                        userRoleId = 3;
                        redirectToController = "Dealer";
                    }
                    else if (user is Customer)
                    {
                        userRoleId = 4;
                        redirectToController = "Customer";
                    }

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel()
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Role = user.GetType().Name,
                        UserType = user.GetType()
                    };
                    string userData = JsonConvert.SerializeObject(serializeModel);

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, user.Username, DateTime.Now, DateTime.Now.AddMinutes(30), false, userData);
                    string encTicket = FormsAuthentication.Encrypt(authTicket);

                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket)
                    {
                        Expires = authTicket.Expiration,
                        Path = FormsAuthentication.FormsCookiePath
                    };
                    Response.Cookies.Add(faCookie);

                    TMSEntities entities = new TMSEntities();
                    var menusList =
                        entities.Menus.Join(entities.RoleMenus.Where(rm => rm.RoleId == userRoleId), m => m.MenuId,
                            r => r.MenuId, (menu, role) => new { menuList = menu })
                            .SelectMany(i => i.menuList.MenuItems)
                            .Where(i => i.ParentMenuId == null)
                            .Include(j => j.Children)
                            .ToList();

                    Session["Menus"] = menusList;
                    Session["UserData"] = user;

                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", redirectToController);
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("Controller : {0} - Action : {1}, Message : {2}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), ex.Message);
                }
            }

            return View(model);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = Mapper.Map<RegisterViewModel, Admin>(model);
                _userService.CreateUser(user);
                _userService.SaveUser();
                return RedirectToAction("Login", "UserAccount", null);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            // Clear authentication cookie
            HttpCookie formsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "")
            {
                Expires = DateTime.Now.AddYears(-1)
            };
            Response.Cookies.Add(formsCookie);

            // Clear session cookie 
            HttpCookie sessionCookie = new HttpCookie("ASP.NET_SessionId", "") { Expires = DateTime.Now.AddYears(-1) };
            Response.Cookies.Add(sessionCookie);

            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            return RedirectToAction("Login", "UserAccount", null);
        }
    }
}


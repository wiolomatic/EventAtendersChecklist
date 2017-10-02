namespace EventAtendersChecklist.Controllers
{
    using EventAtendersChecklist.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using static EventAtendersChecklist.Controllers.ManageController;

    /// <summary>
    /// Defines the <see cref="AccountController" />
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>
        /// Defines the _signInManager
        /// </summary>
        private ApplicationSignInManager _signInManager;

        /// <summary>
        /// Defines the _userManager
        /// </summary>
        private ApplicationUserManager _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        public AccountController()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The <see cref="ApplicationUserManager"/></param>
        /// <param name="signInManager">The <see cref="ApplicationSignInManager"/></param>
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        /// <summary>
        /// Gets or sets the SignInManager
        /// </summary>
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        /// <summary>
        /// Gets or sets the UserManager
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        /// <summary>
        /// The Login
        /// </summary>
        /// <param name="returnUrl">The <see cref="string"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [AllowAnonymous]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        /// <summary>
        /// The Login
        /// </summary>
        /// <param name="model">The <see cref="LoginViewModel"/></param>
        /// <param name="returnUrl">The <see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(Roles = "HR")]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        /// <summary>
        /// The Usmen
        /// </summary>
        /// <returns>The <see cref="ApplicationUserManager"/></returns>
        public ApplicationUserManager Usmen()
        {
            return _userManager;
        }

        /// <summary>
        /// The UM
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Authorize(Roles = "HR")]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult UM()
        {
            UM model = new UM
            {
                dane = new List<UserModelList>()
            };
            var context = new ApplicationDbContext();
            var roles = context.Roles.ToList();
            model.users = UserManager.Users.ToList();

            foreach (var x in model.users)
            {
                UserModelList tymczasowy = new UserModelList
                {
                    NazwaRoli = "",
                    IdRoli = ""
                };
                foreach (var UserRole in x.Roles.ToList())
                {
                    foreach (var m in roles)
                    {

                        if (m.Name != "CP")
                        {
                            if (m.Id == UserRole.RoleId)
                            {
                                tymczasowy.NazwaRoli = m.Name;
                                tymczasowy.IdRoli = m.Id;
                            }
                        }
                    }
                }
                tymczasowy.IdUsera = x.Id;
                tymczasowy.NazwaUsera = x.UserName;
                tymczasowy.Email = x.Email;
                model.dane.Add(tymczasowy);
            }
            return View(model);
        }
        /// <summary>
        /// The UMeditCP
        /// </summary>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        public async Task<ActionResult> UMeditCP()
        {
            string AppUser = User.Identity.GetUserName();
            ApplicationUser appUser = UserManager.FindByName(AppUser);

            var result = await UserManager.RemoveFromRoleAsync(appUser.Id, "CP");
            ViewBag.UserId = "";
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", "Events");
            }
            return View();
        }

        /// <summary>
        /// The UMeditrole
        /// </summary>
        /// <param name="rola">The <see cref="string"/></param>
        /// <param name="user">The <see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize(Roles = "HR")]
        [RoleAuthorize(Roles = "HR")]
        public async Task<ActionResult> UMeditrole(string rola, string user)
        {
            ApplicationUser appUser = UserManager.FindById(user);

            var context = new ApplicationDbContext();
            var DbRoles = context.Roles.ToList();
            string RoleName = "";
            string RoleId = "";
            foreach (var UserRole in appUser.Roles.ToList())
            {
                foreach (var m in DbRoles)
                {

                    if (m.Name != "CP")
                    {
                        if (m.Id == UserRole.RoleId)
                        {
                            RoleName = m.Name;
                            RoleId = m.Id;
                        }
                    }
                }
            }

            await UserManager.RemoveFromRoleAsync(appUser.Id, RoleName);
            await UserManager.AddToRoleAsync(appUser.Id, rola);
            ViewBag.Info = "User with name " + appUser.UserName + " has now new role: " + rola;

            UM model = new UM
            {
                dane = new List<UserModelList>()
            };

            var roles = context.Roles.ToList();
            model.users = UserManager.Users.ToList();
            foreach (var x in model.users)
            {
                UserModelList tymczasowy = new UserModelList
                {
                    NazwaRoli = "",
                    IdRoli = ""
                };
                foreach (var UserRole in x.Roles.ToList())
                {
                    foreach (var m in roles)
                    {
                        if (m.Name != "CP")
                        {
                            if (m.Id == UserRole.RoleId)
                            {
                                tymczasowy.NazwaRoli = m.Name;
                                tymczasowy.IdRoli = m.Id;
                            }
                        }
                    }
                }

                tymczasowy.IdUsera = x.Id;
                tymczasowy.NazwaUsera = x.UserName;
                tymczasowy.Email = x.Email;
                model.dane.Add(tymczasowy);
            }
            return View("UM", model);
        }

        /// <summary>
        /// The UMdeleteUser
        /// </summary>
        /// <param name="rola">The <see cref="string"/></param>
        /// <param name="user">The <see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize(Roles = "HR")]
        [RoleAuthorize(Roles = "HR")]
        public async Task<ActionResult> UMdeleteUser(string rola, string user)
        {
            ApplicationUser appUser = UserManager.FindById(user);

            var context = new ApplicationDbContext();
            var DbRoles = context.Roles.ToList();
            string RoleName = "";
            string RoleId = "";
            foreach (var UserRole in appUser.Roles.ToList())
            {
                foreach (var m in DbRoles)
                {
                    if (m.Name != "CP")
                    {
                        if (m.Id == UserRole.RoleId)
                        {
                            RoleName = m.Name;
                            RoleId = m.Id;
                        }
                    }
                }
            }
            await UserManager.RemoveFromRoleAsync(appUser.Id, RoleName);
            await UserManager.DeleteAsync(appUser);
            ViewBag.Info = "User with name " + appUser.UserName + " has been removed permanently.";
            UM model = new UM
            {
                dane = new List<UserModelList>(),
                users = UserManager.Users.ToList()
            };
            foreach (var x in model.users)
            {
                UserModelList tymczasowy = new UserModelList
                {
                    NazwaRoli = "",
                    IdRoli = x.Roles.First().RoleId
                };

                foreach (var m in DbRoles)
                {
                    if (m.Id == tymczasowy.IdRoli)
                    {
                        tymczasowy.NazwaRoli = m.Name;
                    }
                };

                tymczasowy.IdUsera = x.Id;
                tymczasowy.NazwaUsera = x.UserName;
                tymczasowy.Email = x.Email;
                model.dane.Add(tymczasowy);
            }
            return View("UM", model);
        }

        /// <summary>
        /// The UMedit
        /// </summary>
        /// <param name="id">The <see cref="string"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Authorize(Roles = "HR")]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult UMedit(string id)
        {
            string IdUsera = "";
            IdUsera = id;
            ApplicationUser user = UserManager.FindById(IdUsera);
            var context = new ApplicationDbContext();
            var DbRoles = context.Roles.ToList();
            string RoleName = "";
            string RoleId = "";
            foreach (var UserRole in user.Roles.ToList())
            {
                foreach (var m in DbRoles)
                {
                    if (m.Name != "CP")
                    {
                        if (m.Id == UserRole.RoleId)
                        {
                            RoleName = m.Name;
                            RoleId = m.Id;
                        }
                    }
                }
            }

            ViewBag.userId = IdUsera;
            ViewBag.UserName = user.UserName;
            ViewBag.RoleName = RoleName;
            ViewBag.RoleId = RoleId;

            return View("UMedit");
        }

        /// <summary>
        /// The UMdelete
        /// </summary>
        /// <param name="id">The <see cref="string"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Authorize(Roles = "HR")]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult UMdelete(string id)
        {
            string IdUsera = "";
            IdUsera = id;
            ApplicationUser appUser = UserManager.FindById(IdUsera);
            var context = new ApplicationDbContext();
            var DbRoles = context.Roles.ToList();
            string RoleName = "";
            string RoleId = "";
            foreach (var UserRole in appUser.Roles.ToList())
            {
                foreach (var m in DbRoles)
                {

                    if (m.Name != "CP")
                    {
                        if (m.Id == UserRole.RoleId)
                        {
                            RoleName = m.Name;
                            RoleId = m.Id;
                        }
                    }
                }
            }

            ViewBag.userId = IdUsera;
            ViewBag.UserName = appUser.UserName;
            ViewBag.RoleName = RoleName;
            ViewBag.RoleId = RoleId;
            return View("UMdelete");
        }

        // GET: /Account/VerifyCode
        /// <summary>
        /// The VerifyCode
        /// </summary>
        /// <param name="provider">The <see cref="string"/></param>
        /// <param name="returnUrl">The <see cref="string"/></param>
        /// <param name="rememberMe">The <see cref="bool"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/VerifyCode
        /// <summary>
        /// The VerifyCode
        /// </summary>
        /// <param name="model">The <see cref="VerifyCodeViewModel"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        // GET: /Account/Register
        /// <summary>
        /// The Register
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Authorize(Roles = "HR")]
        [RoleAuthorize(Roles = "HR")]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        /// <summary>
        /// The Register
        /// </summary>
        /// <param name="model">The <see cref="RegisterViewModel"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [Authorize(Roles = "HR")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var context = new ApplicationDbContext();
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(UserManager.FindByName(model.Email).Id, model.Role);
                    UserManager.AddToRole(UserManager.FindByName(model.Email).Id, "CP");
                    return RedirectToAction("UM", "Account");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ConfirmEmail
        /// <summary>
        /// The ConfirmEmail
        /// </summary>
        /// <param name="userId">The <see cref="string"/></param>
        /// <param name="code">The <see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        // GET: /Account/ForgotPassword
        /// <summary>
        /// The ForgotPassword
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        /// <summary>
        /// The ForgotPassword
        /// </summary>
        /// <param name="model">The <see cref="ForgotPasswordViewModel"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
            }
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        /// <summary>
        /// The ForgotPasswordConfirmation
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Authorize]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        /// <summary>
        /// The ResetPassword
        /// </summary>
        /// <param name="code">The <see cref="string"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Authorize]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        /// <summary>
        /// The ResetPassword
        /// </summary>
        /// <param name="model">The <see cref="ResetPasswordViewModel"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        /// <summary>
        /// The ResetPasswordConfirmation
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [Authorize]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // POST: /Account/ExternalLogin
        /// <summary>
        /// The ExternalLogin
        /// </summary>
        /// <param name="provider">The <see cref="string"/></param>
        /// <param name="returnUrl">The <see cref="string"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/SendCode
        /// <summary>
        /// The SendCode
        /// </summary>
        /// <param name="returnUrl">The <see cref="string"/></param>
        /// <param name="rememberMe">The <see cref="bool"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [Authorize]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/SendCode
        /// <summary>
        /// The SendCode
        /// </summary>
        /// <param name="model">The <see cref="SendCodeViewModel"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        // GET: /Account/ExternalLoginCallback
        /// <summary>
        /// The ExternalLoginCallback
        /// </summary>
        /// <param name="returnUrl">The <see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        // POST: /Account/ExternalLoginConfirmation
        /// <summary>
        /// The ExternalLoginConfirmation
        /// </summary>
        /// <param name="model">The <see cref="ExternalLoginConfirmationViewModel"/></param>
        /// <param name="returnUrl">The <see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: /Account/LogOff
        /// <summary>
        /// The LogOff
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/ExternalLoginFailure
        /// <summary>
        /// The ExternalLoginFailure
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        /// <summary>
        /// The Dispose
        /// </summary>
        /// <param name="disposing">The <see cref="bool"/></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }
            base.Dispose(disposing);
        }

        /// Defines the XsrfKey
        /// </summary>
        private const string XsrfKey = "XsrfId";

        /// <summary>
        /// Gets the AuthenticationManager
        /// </summary>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// The AddErrors
        /// </summary>
        /// <param name="result">The <see cref="IdentityResult"/></param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// The RedirectToLocal
        /// </summary>
        /// <param name="returnUrl">The <see cref="string"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Events");
        }

        /// <summary>
        /// Defines the <see cref="ChallengeResult" />
        /// </summary>
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ChallengeResult"/> class.
            /// </summary>
            /// <param name="provider">The <see cref="string"/></param>
            /// <param name="redirectUri">The <see cref="string"/></param>
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ChallengeResult"/> class.
            /// </summary>
            /// <param name="provider">The <see cref="string"/></param>
            /// <param name="redirectUri">The <see cref="string"/></param>
            /// <param name="userId">The <see cref="string"/></param>
            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            /// <summary>
            /// Gets or sets the LoginProvider
            /// </summary>
            public string LoginProvider { get; set; }

            /// <summary>
            /// Gets or sets the RedirectUri
            /// </summary>
            public string RedirectUri { get; set; }

            /// <summary>
            /// Gets or sets the UserId
            /// </summary>
            public string UserId { get; set; }

            /// <summary>
            /// The ExecuteResult
            /// </summary>
            /// <param name="context">The <see cref="ControllerContext"/></param>
            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}

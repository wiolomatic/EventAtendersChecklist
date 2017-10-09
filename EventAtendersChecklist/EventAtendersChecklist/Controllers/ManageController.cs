namespace EventAtendersChecklist.Controllers
{
    using EventAtendersChecklist.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Defines the <see cref="ManageController" />
    /// </summary>
    [Authorize]
    public class ManageController : Controller
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
        /// Initializes a new instance of the <see cref="ManageController"/> class.
        /// </summary>
        public ManageController()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageController"/> class.
        /// </summary>
        /// <param name="userManager">The <see cref="ApplicationUserManager"/></param>
        /// <param name="signInManager">The <see cref="ApplicationSignInManager"/></param>
        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        // GET: /Manage/Index
        /// <summary>
        /// The Index
        /// </summary>
        /// <param name="message">The <see cref="ManageMessageId?"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        /// <summary>
        /// The RemoveLogin
        /// </summary>
        /// <param name="loginProvider">The <see cref="string"/></param>
        /// <param name="providerKey">The <see cref="string"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        // POST: /Manage/EnableTwoFactorAuthentication
        /// <summary>
        /// The EnableTwoFactorAuthentication
        /// </summary>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        /// <summary>
        /// The DisableTwoFactorAuthentication
        /// </summary>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        // GET: /Manage/ChangePassword
        /// <summary>
        /// The ChangePassword
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        /// <summary>
        /// The ChangePassword
        /// </summary>
        /// <param name="model">The <see cref="ChangePasswordViewModel"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                //return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                return RedirectToAction("UMeditCP", "Account");
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        /// <summary>
        /// The SetPassword
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        /// <summary>
        /// The SetPassword
        /// </summary>
        /// <param name="model">The <see cref="SetPasswordViewModel"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// The UMeditCP
        /// </summary>
        /// <returns>The <see cref="ActionResult"/></returns>
        public ActionResult UMeditCP()
        {

            return RedirectToAction("UMeditCP", "Account");
        }

        //
        // GET: /Manage/ManageLogins
        /// <summary>
        /// The ManageLogins
        /// </summary>
        /// <param name="message">The <see cref="ManageMessageId?"/></param>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        /// <summary>
        /// The LinkLogin
        /// </summary>
        /// <param name="provider">The <see cref="string"/></param>
        /// <returns>The <see cref="ActionResult"/></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        /// <summary>
        /// The LinkLoginCallback
        /// </summary>
        /// <returns>The <see cref="Task{ActionResult}"/></returns>
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        /// <summary>
        /// The Dispose
        /// </summary>
        /// <param name="disposing">The <see cref="bool"/></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        // Used for XSRF protection when adding external logins
        /// <summary>
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
        /// The HasPassword
        /// </summary>
        /// <returns>The <see cref="bool"/></returns>
        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        /// <summary>
        /// The HasPhoneNumber
        /// </summary>
        /// <returns>The <see cref="bool"/></returns>
        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        /// <summary>
        /// Defines the ManageMessageId
        /// </summary>
        public enum ManageMessageId
        {
            /// <summary>
            /// Defines the AddPhoneSuccess
            /// </summary>
            AddPhoneSuccess,
            /// <summary>
            /// Defines the ChangePasswordSuccess
            /// </summary>
            ChangePasswordSuccess,
            /// <summary>
            /// Defines the SetTwoFactorSuccess
            /// </summary>
            SetTwoFactorSuccess,
            /// <summary>
            /// Defines the SetPasswordSuccess
            /// </summary>
            SetPasswordSuccess,
            /// <summary>
            /// Defines the RemoveLoginSuccess
            /// </summary>
            RemoveLoginSuccess,
            /// <summary>
            /// Defines the RemovePhoneSuccess
            /// </summary>
            RemovePhoneSuccess,
            /// <summary>
            /// Defines the Error
            /// </summary>
            Error
        }
    }
}

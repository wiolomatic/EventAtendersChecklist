namespace EventAtendersChecklist
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Defines the <see cref="RoleAuthorizeAttribute" />
    /// </summary>
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Defines the redirectUrl
        /// </summary>
        private string redirectUrl = "Events/Index";

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAuthorizeAttribute"/> class.
        /// </summary>
        public RoleAuthorizeAttribute() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="redirectUrl">The <see cref="string"/></param>
        public RoleAuthorizeAttribute(string redirectUrl) : base()
        {
            this.redirectUrl = redirectUrl;
        }

        /// <summary>
        /// The HandleUnauthorizedRequest
        /// </summary>
        /// <param name="filterContext">The <see cref="AuthorizationContext"/></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                string authUrl = this.redirectUrl; //passed from attribute

                //if null, get it from config
                if (String.IsNullOrEmpty(authUrl))
                    authUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["RolesAuthRedirectUrl"];

                if (!String.IsNullOrEmpty(authUrl))
                    filterContext.HttpContext.Response.Redirect(authUrl);
            }
            //else do normal process
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}

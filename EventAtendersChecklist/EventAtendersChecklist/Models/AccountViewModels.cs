namespace EventAtendersChecklist.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="ExternalLoginConfirmationViewModel" />
    /// </summary>
    public class ExternalLoginConfirmationViewModel
    {
        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="ExternalLoginListViewModel" />
    /// </summary>
    public class ExternalLoginListViewModel
    {
        /// <summary>
        /// Gets or sets the ReturnUrl
        /// </summary>
        public string ReturnUrl { get; set; }
    }


    /// <summary>
    /// Defines the <see cref="SendCodeViewModel" />
    /// </summary>
    public class SendCodeViewModel
    {
        /// <summary>
        /// Gets or sets the SelectedProvider
        /// </summary>
        public string SelectedProvider { get; set; }

        /// <summary>
        /// Gets or sets the Providers
        /// </summary>
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }

        /// <summary>
        /// Gets or sets the ReturnUrl
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether RememberMe
        /// </summary>
        public bool RememberMe { get; set; }
    }

    public class UM
    {
        public List<ApplicationUser> users { get; set; }
        public List<UserModelList> dane { get; set; }
        public UserToEdit edit { get; set; }
        public UserToEdit editUser( string Id, string Nazwa)
        {
            UserToEdit ue = new UserToEdit();
            ue.IdUsera = Id;
            ue.NazwaUsera = Nazwa;

            return ue;
        }
    }
    public class UserModelList
    {
        public string IdUsera { get; set; }
        public string NazwaUsera { get; set; }
        public string IdRoli { get; set; }
        public string NazwaRoli { get; set; }
        public string Email { get; set; }

    }
    public class UserToEdit
    {
        public string IdUsera { get; set; }
        public string NazwaUsera { get; set; }
        public string IdRoli { get; set; }
        public string NazwaRoli { get; set; }
        public string NowaRola { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="VerifyCodeViewModel" />
    /// </summary>
    /// 
    public class VerifyCodeViewModel
    {
        /// <summary>
        /// Gets or sets the Provider
        /// </summary>
        [Required]
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets the Code
        /// </summary>
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the ReturnUrl
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether RememberBrowser
        /// </summary>
        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether RememberMe
        /// </summary>
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="ForgotViewModel" />
    /// </summary>
    public class ForgotViewModel
    {
        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="LoginViewModel" />
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether RememberMe
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="RegisterViewModel" />
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the ConfirmPassword
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the Role
        /// </summary>
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="ResetPasswordViewModel" />
    /// </summary>
    public class ResetPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the ConfirmPassword
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the Code
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="ForgotPasswordViewModel" />
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

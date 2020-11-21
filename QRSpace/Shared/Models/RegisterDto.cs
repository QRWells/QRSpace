using System.ComponentModel.DataAnnotations;
// ReSharper disable PropertyNotResolved

namespace QRSpace.Shared.Models
{
    /// <summary>
    ///
    /// </summary>
    public record RegisterDto
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Models.RegisterDto), ErrorMessageResourceName = "UsernameRequireError")]
        [StringLength(32, MinimumLength = 6, ErrorMessageResourceType = typeof(Resources.Models.RegisterDto), ErrorMessageResourceName = "UsernameLengthError")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Models.RegisterDto), ErrorMessageResourceName = "PasswordRequireError")]
        [StringLength(128, MinimumLength = 6, ErrorMessageResourceType = typeof(Resources.Models.RegisterDto), ErrorMessageResourceName = "PasswordLengthError")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Models.RegisterDto), ErrorMessageResourceName = "ConfirmPwdCompareError")]
        public string ConfirmPassword { get; set; }
    }
}
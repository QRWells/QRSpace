using System.ComponentModel.DataAnnotations;

namespace QRSpace.Shared.Models
{
    public record ChangePwdDto
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [MaxLength(128, ErrorMessage = "")]
        [MinLength(6, ErrorMessage = "")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPwd { get; set; }
        
        [Required]
        [MaxLength(128, ErrorMessage = "")]
        [MinLength(6, ErrorMessage = "")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPwd { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("New Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPwd { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace QRSpace.Shared.Models
{
    public record ChangeNameDto
    {
        [Required]
        public string OldUserName { get; set; }
        
        [Required]
        public string NewUserName { get; set; }
        
        [Required]
        [Display(Name = "Password")]
        public string CurrentPwd { get; set; }
    }
}
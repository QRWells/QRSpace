using System.ComponentModel.DataAnnotations;

namespace QRSpace.Shared.Models
{
    public record LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
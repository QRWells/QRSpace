using System.ComponentModel;

namespace QRSpace.Shared.Models
{
    public record UserItemDto
    {
        [DisplayName("ID")]
        public ulong Id { get; set; }

        public string Name { get; set; }
        public string Role { get; set; }
    }
}
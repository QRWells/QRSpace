using System.ComponentModel;

namespace QRSpace.Shared.Models
{
    public class UserItemDto
    {
        [DisplayName("ID")]
        public ulong Id { get; set; }

        public string Name { get; set; }
        public string Role { get; set; }
    }
}
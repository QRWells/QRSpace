using System.ComponentModel.DataAnnotations;

namespace QRSpace.Server.Entities
{
    public class ShogiRecord
    {
        [Required]
        [Key]
        public ulong RecordId { get; set; }

        public string Record { get; set; }

        
        public ApplicationUser User { get; set; }
    }
}
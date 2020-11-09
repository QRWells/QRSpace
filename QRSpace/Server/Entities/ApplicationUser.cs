using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace QRSpace.Server.Entities
{
    public class ApplicationUser : IdentityUser<ulong>
    {
        public ICollection<ShogiRecord> ShogiRecords { get; set; }
    }
}
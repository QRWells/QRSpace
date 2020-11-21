using System.Collections.Generic;

namespace QRSpace.Shared.Models.ActionResults
{
    public record RegisterResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
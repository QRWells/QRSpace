using System;
using System.Collections.Generic;
using System.Text;

namespace QRSpace.Shared.Models.ActionResults
{
    public class RegisterResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
namespace QRSpace.Shared.Models.ActionResults
{
    public record LoginResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
    }
}
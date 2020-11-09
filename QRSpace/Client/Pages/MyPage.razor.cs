using Microsoft.AspNetCore.Components;
using System.Net.Http;

namespace QRSpace.Client.Pages
{
    public class MyPageBase : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        protected string Title { get; set; } = "My Page";
    }
}
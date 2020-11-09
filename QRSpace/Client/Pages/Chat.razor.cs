using Microsoft.AspNetCore.Components;

namespace QRSpace.Client.Pages
{
    public class ChatBase : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            NavigationManager.NavigateTo("/");
        }
    }
}
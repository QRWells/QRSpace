using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;

namespace QRSpace.Client.Pages
{
    public class SettingsBase : ComponentBase
    {
        [Inject] private ILocalStorageService LocalStorage { get; set; }

        [Inject] private HttpClient HttpClient { get; set; }

        protected void OnSaveClicked(MouseEventArgs e)
        {
        }

        protected async void SaveToUserProfileAsync()
        {
            await HttpClient.PostAsync("",new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()));
        }
    }
}
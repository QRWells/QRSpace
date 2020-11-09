using System;
using System.Collections.Generic;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using QRSpace.Client.Authentication;
using QRSpace.Shared.Models;
using QRSpace.Shared.Models.ActionResults;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using QRSpace.Shared.Utils;

namespace QRSpace.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public async Task<LoginResult> LoginAsync(LoginDto loginModel)
        {
            var encryptPwd = EncryptHelper.EncryptWithAES(loginModel.Password);
            var pairs = new Dictionary<string, string>
            {
                {"Username", loginModel.Username},
                {"Password", encryptPwd},
                {"RememberMe", loginModel.RememberMe.ToString()}
            };
            var form = new FormUrlEncodedContent(pairs);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync("api/login", form);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e);
                return new LoginResult { Error = e.Message, Success = false };
            }

            if (!response.IsSuccessStatusCode)
                return new LoginResult { Error = await response.Content.ReadAsStringAsync(), Success = false };
            var content = await response.Content.ReadAsStreamAsync();
            var loginResult = await JsonSerializer.DeserializeAsync<LoginResult>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            await _localStorage.SetItemAsync("authToken", loginResult.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginResult.Token);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult;
        }

        public async Task LogoutAsync()
        {
            await _httpClient.PostAsync("api/logout", new StringContent(""));
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<RegisterResult> RegisterAsync(RegisterDto registerModel)
        {
            var encryptPwd = EncryptHelper.EncryptWithAES(registerModel.ConfirmPassword);
            var pairs = new Dictionary<string, string>
            {
                {"Username", registerModel.Username},
                {"Password", encryptPwd},
                {"ConfirmPassword", encryptPwd}
            };
            var form = new FormUrlEncodedContent(pairs);
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync("api/accounts", form);
            }
            catch (HttpRequestException ex)
            {
                return new RegisterResult { Errors = new List<string> { ex.Message }, Success = false };
            }

            if (!response.IsSuccessStatusCode)
                return new RegisterResult
                { Errors = new List<string> { await response.Content.ReadAsStringAsync() }, Success = false };

            var content = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<RegisterResult>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }
    }
}
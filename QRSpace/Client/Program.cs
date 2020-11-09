using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using QRSpace.Client.Authentication;
using QRSpace.Client.Services;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace QRSpace.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            _ = new JwtHeader();
            _ = new JwtPayload();

            ConfigureServices(builder.Services);

            var host = builder.Build();
            var ls = host.Services.GetRequiredService<ILocalStorageService>();
            var result = await ls.GetItemAsStringAsync("BlazorCulture");
            if (!string.IsNullOrEmpty(result))
            {
                var culture = new CultureInfo(result);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
            await host.RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(sp =>
            {
                var uri = sp.GetService<IConfiguration>().GetSection("App")["ApiUrl"];
                return new HttpClient
                {
                    BaseAddress = new Uri(uri)
                };
            });
            /*
            services.AddSingleton(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                return config.GetSection("App");
            });*/
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddOptions();
            services.AddAuthorizationCore();
            services.AddBlazoredLocalStorage();
            services.AddAntDesign();
            services.AddLocalization(opt => { opt.ResourcesPath = "Resources"; });
        }
    }
}
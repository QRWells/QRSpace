using System;
using System.Globalization;
using System.Threading.Tasks;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using QRSpace.Client.Services;
using QRSpace.Shared.Models;

namespace QRSpace.Client.Shared
{
    public class SideMenuBase : ComponentBase
    {
        protected LoginDto _signInDto = new LoginDto();
        protected RegisterDto _signUpDto = new RegisterDto();

        protected bool SignInLoading;
        protected bool SignInVisible;

        protected bool SignUpLoading;
        protected bool SignUpVisible;

        [Inject] protected NavigationManager Navi { get; set; }
        [Inject] private IAuthService Auth { get; set; }
        [Inject] protected IStringLocalizer<SideMenu> Local { get; set; }

        [Inject] private MessageService MessageService { get; set; }

        protected static ColLayoutParam LabelCol =>
            CultureInfo.CurrentCulture.Name switch
            {
                "zh-CN" => new ColLayoutParam { Span = 4 },
                _ => new ColLayoutParam { Span = 6 }
            };

        protected static ColLayoutParam WrapperCol =>
            CultureInfo.CurrentCulture.Name switch
            {
                "zh-CN" => new ColLayoutParam { Span = 18 },
                _ => new ColLayoutParam { Span = 16 }
            };

        protected static ColLayoutParam SubmitBtnCol => CultureInfo.CurrentCulture.Name switch
        {
            "zh-CN" => new ColLayoutParam { Offset = 4, Span = 8 },
            _ => new ColLayoutParam { Offset = 6, Span = 8 }
        };

        protected string ButtonsStyle => !Collapsed ? "margin:16px;display:flex;" : "display:none;";

        protected bool Collapsed { get; set; }

        [Parameter] public EventCallback<bool> OnCollapseChanged { get; set; }

        protected void OnCollapse(bool collapsed)
        {
            Collapsed = collapsed;
            if (OnCollapseChanged.HasDelegate) OnCollapseChanged.InvokeAsync(Collapsed);
        }

        protected async void OnLogoutClicked()
        {
            await Auth.LogoutAsync();
        }

        protected async Task OnSignUpSubmit()
        {
            SignUpLoading = true;
            var result = await Auth.RegisterAsync(_signUpDto);
            if (!result.Success)
            {
                SignUpLoading = false;
                await MessageService.Error(Local["SignUpFailed"].Value);
            }
            else
            {
                await MessageService.Success("Sign up successfully!");
                SignUpLoading = false;
                SignUpVisible = false;
            }
        }

        protected void OnSignUpFailed() => MessageService.Error(Local["SignUpFailed"].Value);

        protected async Task OnSignInSubmit()
        {
            SignInLoading = true;
            var result = await Auth.LoginAsync(_signInDto);
            if (!result.Success)
            {
                SignInLoading = false;
                await MessageService.Error(Local["SignInFailed"].Value);
            }
            else
            {
                await MessageService.Success("Login successful");
                SignInLoading = false;
                SignInVisible = false;
            }
        }

        protected void OnSignInFailed() => MessageService.Error("登录失败");

        protected void OnSignInCancel() => SignInVisible = false;

        protected void OnSignUpCancel() => SignUpVisible = false;
    }
}
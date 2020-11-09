using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using QRSpace.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QRSpace.Client.Pages
{
    public class AdminBase : ComponentBase
    {
        [Inject]
        protected NavigationManager Navi { get; set; }

        [Inject]
        protected HttpClient Http { get; set; }

        [Parameter]
        public ulong UserId { get; set; }

        public ITable table;

        public List<UserItemDto> UserList = new List<UserItemDto>();

        public int _pageIndex = 1;
        public int _pageSize = 10;
        public int _total = 0;

        public bool _loading = true;

        protected async override Task OnInitializedAsync()
        {
            var u = await Http.GetFromJsonAsync<IEnumerable<UserItemDto>>("api/admin/users");
            if (u != null)
            {
                UserList = u.ToList();
                _loading = false;
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }

        protected void OnChange(QueryModel<UserItemDto> queryModel)
        {
        }

        protected async Task<IEnumerable<UserItemDto>> FetchData(PaginationEventArgs args)
        {
            var result = await Http.GetFromJsonAsync<IEnumerable<UserItemDto>>("api/admin/users");
            if (result != null)
            {
                _loading = false;
                return result;
            }
            else
            {
                return new List<UserItemDto>();
            }
        }

        protected void OnEditClick(ulong Id)
        {
            //TODO
        }

        protected void OnDeleteClick(ulong Id)
        {
            //TODO
        }
    }
}
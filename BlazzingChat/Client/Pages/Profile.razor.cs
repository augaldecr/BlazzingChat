using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using BlazzingChat.Client.ViewModels;
using System.Net.Http;
using Microsoft.JSInterop.Implementation;

namespace BlazzingChat.Client.Pages
{
    public partial class Profile : ComponentBase
    {
        [Inject] IProfileViewModel _profileViewModel { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }
        [Inject] IJSRuntime _jsruntime {  get; set; }
        [Inject] HttpClient _httpClient { get; set; }

        private IJSObjectReference _jsUtils;

        [CascadingParameter] Task<AuthenticationState> _authenticationState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await _authenticationState;//.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                var claim = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
                _profileViewModel.Id = Convert.ToInt32(claim?.Value);

                try
                {
                    await _profileViewModel.GetProfile();
                }
                catch (System.Net.Http.HttpRequestException hex)
                {
                    if (hex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await _httpClient.GetAsync("api/Users/logoutuser");
                        _navigationManager.NavigateTo("/", true);
                    }
                }
            }
            else
            {
                _navigationManager.NavigateTo("/");
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsUtils = await _jsruntime.InvokeAsync<IJSObjectReference>("import", "./js/site.js");
            }
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs inputFileChangeEventArgs)
        {
            //get the file
            var file = inputFileChangeEventArgs.File;
            
            //read the file in a byte array 
            var buffer = new byte[file.Size];
            await file.OpenReadStream().ReadAsync(buffer);

            //convert byte array to base 64 string
            _profileViewModel.ProfilePictDataUrl = $"data:image/png;base64,{Convert.ToBase64String(buffer)}";
        }
    
        private async Task DownloadProfilePicture()
        {
            string[] base64String = _profileViewModel.ProfilePictDataUrl.Split(',');
            await _jsUtils.InvokeVoidAsync("downloadFile", "image/png", base64String[1], "profile.png");
        }

        private async Task DownloadServerFile()
        {
            var httpResponseMessage = await _httpClient.GetAsync("api/Users/DownloadServerFile");
            var base64String = httpResponseMessage.Content.ReadAsStringAsync().Result;

            await _jsUtils.InvokeVoidAsync("downloadFile",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                base64String, "idl_estrategias.docx");
        }
    }
}

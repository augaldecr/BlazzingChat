using BlazzingChat.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazzingChat.Client.Pages
{
    public partial class Chat
    {
        [CascadingParameter] Task<AuthenticationState> _authenticationStateTask { get; set; }
        [Parameter] public string ToUserId { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }
        [Inject] HttpClient _httpClient { get; set; }
        [Inject] IJSRuntime _jSRuntime { get; set; }

        IJSObjectReference module;

        public User ToUser { get; set; } = new();
        public string FromUserId { get; set; }
        public string MessageText { get; set; }
        public List<Message> Messages { get; set; } = new ();

        private HubConnection hubConnection;

        protected override async Task OnInitializedAsync()
        {
            var claimsPrincipal = (await _authenticationStateTask).User;

            if (!claimsPrincipal.Identity.IsAuthenticated)
                _navigationManager.NavigateTo("/");

            FromUserId = (await _httpClient.GetFromJsonAsync<User>("api/Users/getcurrentuser")).Id.ToString();

            if (Convert.ToInt32(ToUserId) > 0)
                ToUser = await _httpClient.GetFromJsonAsync<User>($"api/Users/{ToUserId}");

            hubConnection = new HubConnectionBuilder()
                .WithUrl(_navigationManager.ToAbsoluteUri("/chathub"))
                .Build();

            hubConnection.On<Message>("ReceiveMessage", (message) =>
            {
                if (message.FromUserId == FromUserId || 
                (message.ToUserId == FromUserId && message.FromUserId == ToUserId ))
                {
                    Messages.Add(message);
                    StateHasChanged();
                }
            });

            await hubConnection.StartAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
                module = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/site.js");
            
            await module.InvokeVoidAsync("setScroll");
        }

        public async Task Send()
        {
            Message message = new Message();
            message.ToUserId = ToUserId;
            message.FromUserId = FromUserId;
            message.MessageText = MessageText;

            await hubConnection.SendAsync("SendMessage", message);
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Blazored.Toast.Services;

namespace BlazzingChat.Client.Shared
{
    public partial class Error : ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Inject] private ILogger<Error> Logger { get; set; }
        [Inject] private IToastService ToastService { get; set; }
        [Inject] IWebAssemblyHostEnvironment WebAssemblyHostEnvironment {  get; set; }

        public void ProcessError(Exception ex)
        {
            if (WebAssemblyHostEnvironment.IsProduction())
            {
                ToastService.ShowError("Something has gone wrong!");
                Logger.LogError(ex, string.Empty);
            }
            else
            {
                ToastService.ShowError(ex.Message);
            }
        }
    }
}

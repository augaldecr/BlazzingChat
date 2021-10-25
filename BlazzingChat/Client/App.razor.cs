using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazzingChat.Client
{
    public partial class App
    {
        [Inject] Microsoft.AspNetCore.Components.WebAssembly.Services.LazyAssemblyLoader AssemblyLoader { get; set; }

        private List<Assembly> lazyLoadedAssemblies = new();

        private async Task OnNavigation(NavigationContext navigationContext)
        {
            if (navigationContext.Path == "/settings")
            {
                var assemblies = await AssemblyLoader.LoadAssembliesAsync(new[] { "Radzen.Blazor.dll" });
                lazyLoadedAssemblies.AddRange(assemblies);
            }
        }
    }
}

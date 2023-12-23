using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Pomodorium.Shared;

public partial class LoginDisplay
{
    private async Task BeginLogout(MouseEventArgs args)
    {
        Navigation.NavigateToLogout("authentication/logout");

        await Task.CompletedTask;
    }
}

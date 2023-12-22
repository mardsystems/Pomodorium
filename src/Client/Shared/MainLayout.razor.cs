using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Pomodorium.Shared;

public partial class MainLayout
{
    private readonly MudTheme _theme = new()
    {
        Palette = new PaletteLight()
        {
            Primary = Colors.Green.Darken2,
            Secondary = Colors.Red.Darken2,
            AppbarBackground = Colors.Red.Darken1
        }
    };

    // private bool _isHubConnected;

    private bool _isDarkMode;

    bool _drawerOpen = true;

    // private long NotificationTotal = 0;

    protected override async Task OnInitializedAsync()
    {
        // EventHubClient.Notification.Subscribe((e) =>
        // {
        //     NotificationTotal += 1;

        //     InvokeAsync(StateHasChanged);
        // });

        // if (HubConnection.State == HubConnectionState.Connected)
        // {
        //     _isHubConnected = true;
        // }
        // else
        // {
        //     _isHubConnected = false;
        // }

        await Task.CompletedTask;
    }

    // private async Task OnToogleHubConnection(bool toogled)
    // {
    //     try
    //     {
    //         if (HubConnection.State == HubConnectionState.Disconnected)
    //         {
    //             await HubConnection.StartAsync();

    //             _isHubConnected = true;
    //         }
    //         else if (HubConnection.State == HubConnectionState.Connected)
    //         {
    //             await HubConnection.StopAsync();

    //             _isHubConnected = false;
    //         }
    //     }
    //     catch (Exception)
    //     {
    //         _isHubConnected = false;
    //     }
    // }

    // private async Task DispatchEvents()
    // {
    //     await EventHubClient.DispatchEvents();
    // }

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}

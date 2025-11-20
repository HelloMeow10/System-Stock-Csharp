using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Presentation.Services;

public class SignalRClient : IDisposable
{
    private readonly HubConnection _connection;

    public SignalRClient(string hubUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _connection.On<object>("ProductCreated", (payload) =>
        {
            try
            {
                // Simple notification: show a MessageBox on product creation
                // We marshal to the UI thread via Invoke if required by WinForms caller.
                System.Windows.Forms.MessageBox.Show("Se creó/actualizó un producto. Actualice la vista.", "Notificación", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch { }
        });
    }

    public async Task StartAsync()
    {
        try
        {
            await _connection.StartAsync();
        }
        catch { }
    }

    public void Dispose()
    {
        try { _connection.StopAsync().GetAwaiter().GetResult(); } catch { }
        _connection.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}

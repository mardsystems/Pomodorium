using Microsoft.JSInterop;
using System.DomainModel.Storage;

namespace Pomodorium.Data;

public class IndexedDBAccessor : IAsyncDisposable
{
    private Lazy<IJSObjectReference> _accessorJsRef = new();

    private readonly IJSRuntime _jsRuntime;

    public IndexedDBAccessor(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        await WaitForReference();

        await _accessorJsRef.Value.InvokeVoidAsync("initialize");
    }

    public async Task<T[]> GetAllAsync<T>(string collectionName)
    {
        await WaitForReference();

        var result = await _accessorJsRef.Value.InvokeAsync<T[]>("getAll", collectionName);

        return result;
    }

    public async Task<EventRecord[]> GetEventsAsync(string name)
    {
        await WaitForReference();

        var result = await _accessorJsRef.Value.InvokeAsync<EventRecord[]>("getEvents", name);

        return result;
    }

    public async Task<T> GetAsync<T>(string collectionName, Guid id)
    {
        await WaitForReference();

        var result = await _accessorJsRef.Value.InvokeAsync<T>("get", collectionName, id);

        return result;
    }

    public async Task PutAsync<T>(string collectionName, T value)
    {
        await WaitForReference();

        await _accessorJsRef.Value.InvokeVoidAsync("put", collectionName, value);
    }

    public async Task RemoveAsync(string collectionName, Guid id)
    {
        await WaitForReference();

        await _accessorJsRef.Value.InvokeVoidAsync("remove", collectionName, id);
    }

    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false)
        {
            _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/indexedDBAccessor.js?123"));
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_accessorJsRef.IsValueCreated)
        {
            await _accessorJsRef.Value.DisposeAsync();
        }
    }
}
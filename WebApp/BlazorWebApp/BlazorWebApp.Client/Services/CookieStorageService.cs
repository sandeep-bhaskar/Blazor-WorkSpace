using BlazorWebApp.Client.Constants;
using Microsoft.JSInterop;

namespace BlazorWebApp.Client.Services
{
    public class CookieStorageService(IJSRuntime jsRuntime) : ICookieStorageService
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        public async Task AddItemAsync(string key, string value)
        {
            var options = new { expires = 2 };
            await _jsRuntime.InvokeVoidAsync(ClientConstants.CookieStorage.SetItem, key, value, options);
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync(ClientConstants.CookieStorage.RemoveItem, key);
        }

        public async Task<string> GetItemAsync(string key)
        {
            return await _jsRuntime.InvokeAsync<string>(ClientConstants.CookieStorage.GetItem, key);
        }
    }
}

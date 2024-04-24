namespace BlazorWebApp.Client.Services
{
    public interface ICookieStorageService
    {
        Task AddItemAsync(string key, string value);
        Task RemoveItem(string key);
        Task<string> GetItemAsync(string key);
    }
}

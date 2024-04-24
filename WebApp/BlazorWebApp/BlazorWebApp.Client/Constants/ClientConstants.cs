namespace BlazorWebApp.Client.Constants
{
    public static class ClientConstants
    {
        public static class CookieStorage
        {
            public static readonly string AuthToken = "X-Access-Token";
            public static readonly string SetItem = "cookieFunctions.setCookie";
            public static readonly string RemoveItem = "cookieFunctions.removeCookie";
            public static readonly string GetItem = "cookieFunctions.getCookie";
        }
    }
}

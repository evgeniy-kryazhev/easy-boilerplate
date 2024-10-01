namespace EasyBoilerplate.Application.Helpers;

public static class QueryStringHelper
{
    public static string BuildUrlWithQueryStringUsingUriBuilder(string basePath, Dictionary<string, string> queryParams)
    {
        var uriBuilder = new UriBuilder(basePath)
        {
            Query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"))
        };
        return uriBuilder.Uri.AbsoluteUri;
    }
}
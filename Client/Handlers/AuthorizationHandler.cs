using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace Client.Handlers;

public class AuthorizationHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public AuthorizationHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken ct)
    {
        var token = _localStorage.GetItem<string>("authToken");

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, ct);
    }
}
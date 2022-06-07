using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Brigade.Core.Events;
using Brigade.Exceptions;
using Brigade.Shared;
using Microsoft.AspNetCore.WebUtilities;

namespace Brigade.Rest;

public class BaseClient
{
    private readonly HttpClient _httpClient;

    protected BaseClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected Task<TResponse> Post<TResponse, TBody>(PostRequest request)
    {
        request.Body.ApiVersion = Constants.ApiVersion;
        if (!string.IsNullOrWhiteSpace(request.BodyKind))
        {
            request.Body.Kind = request.BodyKind;
        }

        var message = BuildMessage(request);

        message.Content = JsonContent.Create(request.Body, typeof(TBody));

        return Send<TResponse>(request, message);
    }

    private async Task<T> Send<T>(Request request, HttpRequestMessage requestMessage)
    {
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        foreach (var requestHeader in request.Headers)
        {
            requestMessage.Headers.Add(requestHeader.Key, requestHeader.Value);
        }

        var response = await _httpClient.SendAsync(requestMessage);

        if (response.StatusCode == request.SuccessCode)
        {
            return (await response.Content.ReadFromJsonAsync<T>())!;
        }

        throw await HandleResponse(response);
    }

    private static HttpRequestMessage BuildMessage(Request request)
    {
        if (!string.IsNullOrWhiteSpace(request.ListOptions?.Continue))
        {
            request.QueryParams["continue"] = request.ListOptions.Continue;
        }

        if (request.ListOptions?.Limit != null)
        {
            request.QueryParams["limit"] = request.ListOptions.Limit.ToString();
        }

        var path = QueryHelpers.AddQueryString(request.Path, request.QueryParams);

        return new HttpRequestMessage(request.Method, path);
    }

    protected static async Task<Exception> HandleResponse(HttpResponseMessage response)
    {
        var content = (await response.Content.ReadFromJsonAsync<FailedRequest>())!;

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                return new ApiException($"Could not authenticate the request: {content.Reason}");
            case HttpStatusCode.Forbidden:
                return new ApiException("The request is not authorized.");
            case HttpStatusCode.BadRequest:
                var msg = new StringBuilder($"Bad request: {content.Reason}");
                for (var i = 0; i < content.Details.Length; i++)
                {
                    msg.AppendLine($"{i}. {content.Details[i]}");
                }
                return new ApiException(msg.ToString());
            case HttpStatusCode.NotFound:
                return new ApiException($"{content.Type} \"{content.Id}\" not found.");
            case HttpStatusCode.Conflict:
                return new ApiException(content.Reason);
            case HttpStatusCode.InternalServerError:
                return new ApiException("An internal server error occurred.");
            case HttpStatusCode.NotImplemented:
                return new ApiException($"Request not supported: {content.Details}");
            default:
                return new ApiException($"received {response.StatusCode} from API server");
        }
    }
}
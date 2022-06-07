using System.Net;
using Brigade.Meta;

namespace Brigade.Rest;

public class Request
{
    public Request(HttpMethod method, string path)
    {
        Method = method;
        Path = path;
    }

    public HttpMethod Method { get; }
    public string Path { get; }
    public Dictionary<string, string> Headers { get; set; } = new();
    public Dictionary<string, string> QueryParams { get; set; } = new();
    public bool Https { get; set; }
    public IListOptions? ListOptions { get; set; }
    public HttpStatusCode SuccessCode { get; set; } = HttpStatusCode.OK;
}

public class PostRequest : Request
{
    public PostRequest(string path, IRequestBody body) : base(HttpMethod.Post, path)
    {
        Body = body;
    }

    public IRequestBody Body { get; }
    public string? BodyKind { get; set; }
}
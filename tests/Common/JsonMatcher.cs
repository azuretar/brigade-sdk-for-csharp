using System.Net.Http;
using System.Net.Http.Json;
using RichardSzalay.MockHttp;

namespace tests.core;

public class JsonMatcher<T> : IMockedRequestMatcher
{
    private readonly string _content;

    public JsonMatcher(T content)
    {
        _content = JsonContent.Create(content).ReadAsStringAsync().Result;
    }

    public bool Matches(HttpRequestMessage message)
    {
        if(message.Content == null)
            return false;

        var content = message.Content.ReadAsStringAsync().Result;

        return content == _content;
    }
}
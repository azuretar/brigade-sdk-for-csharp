using System.Text.Json.Serialization;

namespace Brigade.Rest;

public class FailedRequest
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("details")]
    public string[]? Details { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
namespace Brigade.Rest;

public interface IRequestBody
{
    string Kind { get; set; }
    string ApiVersion { get; set; }
}
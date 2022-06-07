namespace Brigade.Meta;

public interface IObjectMeta
{
    public string Id { get; set; }
    public DateTime? Created { get; set; }
}

public class ObjectMeta : IObjectMeta
{
    public string? Id { get; set; }
    public DateTime? Created { get; set; }
}
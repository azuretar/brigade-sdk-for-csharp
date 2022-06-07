namespace Brigade.Meta;

public interface IListMeta
{
    string? Continue { get; set; }
    int? RemainingItemCount { get; set; }
}

public class ListMeta : IListMeta
{
    public string? Continue { get; set; }
    public int? RemainingItemCount { get; set; }
}

public interface IListOptions
{
    string Continue { get; set; }
    int Limit { get; set; }
}
namespace meta;

public interface IMetaList<T> : IList<T>   
{
    public IListMeta? Metadata { get; set; }
    public List<T> Items { get; set; }
}

public interface IListMeta
{
    string? Continue { get; set; }
    int? RemainingItemCount { get; set; }
}

public interface IListOptions
{
    string Continue { get; set; }
    int Limit { get; set; }
}
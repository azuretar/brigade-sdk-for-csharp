namespace Brigade.Meta;

public interface IMetaList<T>
{
    public IListMeta? Metadata { get; set; }
    public List<T> Items { get; set; }
}

public class MetaList<T> : IMetaList<T>
{
    public IListMeta? Metadata { get; set; }
    public List<T> Items { get; set; }
}
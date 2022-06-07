namespace Brigade.Core.Events;

/**
 * Encapsulates opaque, source-specific (e.g. gateway-specific) state.
 */
public interface ISourceState
{
    /**
	 * A map of arbitrary and opaque key/value pairs that the source of an Event
	 * (e.g. the gateway that created it) can use to store source-specific state.
	 */
    IDictionary<string, string>? State { get; set; }
}

public class SourceState : ISourceState
{
    public IDictionary<string, string>? State { get; set; } = new Dictionary<string, string>();
}
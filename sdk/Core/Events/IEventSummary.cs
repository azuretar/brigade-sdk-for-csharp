namespace Brigade.Core.Events;

/**
 * Encapsulates an opaque, Worker-specific summary of an Event.
 */
public interface IEventSummary
{
    /**
	 * The Event summary as (optionally) provided by a Worker.
	 */
    string Text { get; set; }
}
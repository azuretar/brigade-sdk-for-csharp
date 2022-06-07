namespace Brigade.Core.Events;

/**
 * A summary of a mass Event cancellation operation.
 */
public interface ICancelManyEventsResult
{
    // The number of Events canceled
    int Count { get; set; }
}
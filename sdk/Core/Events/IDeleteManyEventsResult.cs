namespace Brigade.Core.Events;

/**
 * A summary of a mass Event deletion operation.
 */
public interface IDeleteManyEventsResult
{
    // The number of Events deleted
    int Count { get; set; }
}
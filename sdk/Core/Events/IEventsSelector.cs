namespace Brigade.Core.Events;

/**
 * Useful filter criteria when selecting multiple Events for API group
 * operations like list, cancel, or delete.
 */
public interface IEventsSelector
{
    /**
	 * Specifies that Events belonging to the indicated Project should be selected
	 */
    string? ProjectID { get; set; }
    /**
	 * Specifies that only Events from the indicated source should be selected.
	 */
    string? Source { get; set; }
    /**
	  * Specifies that only Events having all of the indicated source state
	  * key/value pairs should be selected.
	  */
    IDictionary<string, string>? SourceState { get; set; }

    /**
	 * Specifies that only Events having the indicated type should be selected.
	 */
    string? Type { get; set; }
    /**
	 * Specifies that Events with their Workers in any of the indicated phases
	 * should be selected
	 */
    //[]?workerPhases ?: WorkerPhase[] { get; set; }
    /**
	 * Specifies that only Events qualified with these key/value pairs should be
	 * selected.
	 */
    IDictionary<string, string>? Qualifiers { get; set; }
    /**
	 * Specifies that only Events labeled with these key/value pairs should be
	 * selected.
	 */
    IDictionary<string, string>? Labels { get; set; }
}
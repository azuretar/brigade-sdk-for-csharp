using meta;

namespace sdk;

public interface IEventClient
{
	Task<IListMeta> Create(IEvent eventItem);
	Task<IListMeta> List(IEventsSelector? selector, IListOptions? opts);
	Task<IEvent> Get(string id);
	Task<IEvent> Clone(string id);
	Task UpdateSummary(string id, IEventSummary summary);
	Task<IEvent> Retry(string id);
	Task Cancel(string id);
	Task<ICancelManyEventsResult> CancelMany(IEventsSelector selector);
	Task Delete(string id);
	Task<IDeleteManyEventsResult> DeleteMany(IEventsSelector selector);
	//IWorkersClient Workers();
	//ILogsClient Logs();
}

public class EventClient : IEventClient
{
    public Task<IListMeta> Create(IEvent eventItem)
    {
        throw new NotImplementedException();
    }

    public Task<IListMeta> List(IEventsSelector? selector, IListOptions? opts)
    {
        throw new NotImplementedException();
    }

    public Task Cancel(string id)
    {
        throw new NotImplementedException();
    }

    public Task<ICancelManyEventsResult> CancelMany(IEventsSelector selector)
    {
        throw new NotImplementedException();
    }

    public Task<IEvent> Clone(string id)
    {
        throw new NotImplementedException();
    }

    public Task Delete(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IDeleteManyEventsResult> DeleteMany(IEventsSelector selector)
    {
        throw new NotImplementedException();
    }

    public Task<IEvent> Get(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEvent> Retry(string id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSummary(string id, IEventSummary summary)
    {
        throw new NotImplementedException();
    }
	private IDictionary<string, string> EventsSelectorToQueryParams(IEventsSelector selector)
	{
		var queryParams = new Dictionary<string, string>();
		if (selector.ProjectID != null)
		{
			queryParams.Add("projectID", selector.ProjectID);
		}
		if (selector.Source != null)
		{
			queryParams.Add("source", selector.Source);
		}
		if (selector.Qualifiers != null) 
		{
			var qualifiersStrs = new List<string>();
			foreach (var qualifier in selector.Qualifiers)
            {
				qualifiersStrs.Add(qualifier.ToString());
            }
			queryParams.Add("qualifiers", string.Join(",", qualifiersStrs));
		}
		if (selector.Labels != null)
		{
			var labelsStrs = new List<string>();
			foreach (var labels in selector.Labels)
			{
				labelsStrs.Add(labels.ToString());
			}
			queryParams.Add("labels", string.Join(",", labelsStrs));
		}
		if (selector.SourceState != null)
		{
			var sourceStateStrs = new List<string>();
			foreach (var sourceState in selector.SourceState)
			{
				sourceStateStrs.Add(sourceState.ToString());
			}
			queryParams.Add("sourceState", string.Join(",", sourceStateStrs));
		}
		if (selector.Type != null)
		{
			queryParams.Add("type", selector.Type);
		}
		//if (selector.WorkerPhases != null)
		//{
		//	queryParams.Add("workerPhases", string.Join(",", selector.WorkerPhases));
	    //}
		return queryParams;
	}

}

public interface IEvent
{
	  /**
	  * Contains Event metadata
	  */
	  IObjectMeta? Metadata { get; set; }

		/**
	   * Specifies the Project this Event is for. Often, this field will be left
	   * blank when creating a new Event, in which case the Event is matched against
	   * subscribed Projects on the basis of the Source, Type, and Labels fields,
	   * then used as a template to create a discrete Event for each subscribed
	   * Project.
	   */
	  string? ProjectID { get; set; }
	  /**
	   * Specifies the source of the event, e.g. what gateway created it. Gateways
	   * should populate this field with a unique string that clearly identifies
	   * itself as the source of the event. The ServiceAccount used by each gateway
	   * can be authorized (by an administrator) to only create events having a
	   * specified value in the Source field, thereby eliminating the possibility of
	   * gateways maliciously creating events that spoof events from another
	   * gateway.
	   */
	  string Source { get; set; }
	  /**
	   * Encapsulates opaque, source-specific (e.g. gateway-specific) state.
	   */
	  ISourceState? SourceState { get; set; }
	  /**
	   * Specifies the exact event that has occurred in the upstream system. Values
	   * are opaque and source-specific.
	   */
	  string Type { get; set; }
	  /**
	   * Provides critical disambiguation of an Event's type. A Project is
	   * considered subscribed to an Event IF AND ONLY IF (in addition to matching
	   * the Event's Source and Type) it matches ALL of the Event's qualifiers
	   * EXACTLY. To demonstrate the usefulness of this, consider any event from a
	   * hypothetical GitHub gateway. If, by design, that gateway does not intend
	   * for any Project to subscribe to ALL Events (i.e. regardless of which
	   * repository they originated from), then that gateway can QUALIFY Events it
	   * emits into Brigade's event bus with repo=<repository name>. Projects
	   * wishing to subscribe to Events from the GitHub gateway MUST include that
	   * Qualifier in their EventSubscription. Note that the Qualifiers field's
	   * "MUST match" subscription semantics differ from the Labels field's "MAY
	   * match" subscription semantics.
	   */
	  IDictionary<string, string>? Qualifiers { get; set; }
	  /**
	   * Conveys supplementary Event details that Projects may OPTIONALLY use
	   * to narrow EventSubscription criteria. A Project is considered subscribed to
	   * an Event if (in addition to matching the Event's Source, Type, and
	   * Qualifiers) the Event has ALL labels expressed in the Project's
	   * EventSubscription. If the Event has ADDITIONAL labels, not mentioned by the
	   * EventSubscription, these do not preclude a match. To demonstrate the
	   * usefulness of this, consider any event from a hypothetical Slack gateway.
	   * If, by design, that gateway intends for Projects to select between
	   * subscribing to ALL Events or ONLY events originating from a specific
	   * channel, then that gateway can LABEL Events it emits into Brigade's event
	   * bus with channel=<channel name>. Projects wishing to subscribe to ALL
	   * Events from the Slack gateway MAY omit that Label from their
	   * EventSubscription, while Projects wishing to subscribe to only Events
	   * originating from a specific channel MAY include that Label in their
	   * EventSubscription. Note that the Labels field's "MAY match" subscription
	   * semantics differ from the Qualifiers field's "MUST match" subscription
	   * semantics.
	   */
	  IDictionary<string, string>? Labels { get; set; }
	  /**
	   * An optional, succinct title for the Event, ideal for use in lists or in
	   * scenarios where UI real estate is constrained.
	   */
	  string? ShortTitle { get; set; }
	  /**
	   * An optional, detailed title for the Event
	   */
	  string? LongTitle { get; set; }
	  /**
	   * If applicable, contains git-specific Event details. These can be used to override
	   * similar details defined at the Project level. This is useful for scenarios
	   * wherein an Event may need to convey an alternative source, branch, etc.
	   */
	  IGitDetails? Git { get; set; }
	  /**
	   * Optionally contains Event details provided by the upstream system that was
	   * the original source of the event. Payloads MUST NOT contain sensitive
	   * information. Since Projects SUBSCRIBE to Events, the potential exists for
	   * any Project to express an interest in any or all Events. This being the
	   * case, sensitive details must never be present in payloads. The common
	   * workaround for this constraint (which is also a sensible practice to begin
	   * with) is that event payloads may contain REFERENCES to sensitive details
	   * that are useful only to properly configured Workers.
	   */
	  string? Payload { get; set; }
	  /**
	   * A counterpart to payload. If payload is free-form Worker input,
		 * then Summary is free-form Worker output. It can optionally be set by a
		 * Worker to provide a summary of the work completed by the Worker and its
		 * Jobs.
	   */
	  string? Summary { get; set; }
	  /**
	   * Contains details of the Worker assigned to handle the Event
	   */
	  //worker?: worker { get; set; }

}

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

/**
 * Represents git-specific Event details. These may override Project-level
 * GitConfig.
 */
public interface IGitDetails
{
	/**
	 * Specifies the location from where a source code repository may be cloned
	 */
	string? CloneURL { get; set; }
	/**
	 * Specifies a commit (by sha) to be checked out. If specified, takes
	 * precedence over any tag or branch specified by the ref field.
	 */
	string? Commit { get; set; }
	/**
	 * Specifies a tag or branch to be checked out. If left blank, this will
	 * default to "master" at runtime.
	 */
	string? Reference { get; set; }
}

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

/**
 * A summary of a mass Event cancellation operation.
 */
public interface ICancelManyEventsResult
{
	// The number of Events canceled
	int Count { get; set; }
}

/**
 * A summary of a mass Event deletion operation.
 */
public interface IDeleteManyEventsResult
{
	// The number of Events deleted
	int Count { get; set; }
}

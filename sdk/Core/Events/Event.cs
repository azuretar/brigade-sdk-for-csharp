using System.Text.Json.Serialization;
using Brigade.Json;
using Brigade.Meta;
using Brigade.Rest;

namespace Brigade.Core.Events;

public class Event: IRequestBody
{
    /**
	  * Contains Event metadata
	  */
	[JsonConverter(typeof(ConcreteTypeConverter<ObjectMeta, IObjectMeta>))]
    public IObjectMeta? Metadata { get; set; }

    /**
	   * Specifies the Project this Event is for. Often, this field will be left
	   * blank when creating a new Event, in which case the Event is matched against
	   * subscribed Projects on the basis of the Source, Type, and Labels fields,
	   * then used as a template to create a discrete Event for each subscribed
	   * Project.
	   */
    public string? ProjectID { get; set; }
    /**
	   * Specifies the source of the event, e.g. what gateway created it. Gateways
	   * should populate this field with a unique string that clearly identifies
	   * itself as the source of the event. The ServiceAccount used by each gateway
	   * can be authorized (by an administrator) to only create events having a
	   * specified value in the Source field, thereby eliminating the possibility of
	   * gateways maliciously creating events that spoof events from another
	   * gateway.
	   */
    public string Source { get; set; }
	/**
	   * Encapsulates opaque, source-specific (e.g. gateway-specific) state.
	   */
    [JsonConverter(typeof(ConcreteTypeConverter<SourceState, ISourceState>))]
	public ISourceState? SourceState { get; set; }
    /**
	   * Specifies the exact event that has occurred in the upstream system. Values
	   * are opaque and source-specific.
	   */
    public string Type { get; set; }
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
    public IDictionary<string, string>? Qualifiers { get; set; }
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
    public IDictionary<string, string>? Labels { get; set; }
    /**
	   * An optional, succinct title for the Event, ideal for use in lists or in
	   * scenarios where UI real estate is constrained.
	   */
    public string? ShortTitle { get; set; }
    /**
	   * An optional, detailed title for the Event
	   */
    public string? LongTitle { get; set; }
	/**
	   * If applicable, contains git-specific Event details. These can be used to override
	   * similar details defined at the Project level. This is useful for scenarios
	   * wherein an Event may need to convey an alternative source, branch, etc.
	   */
    [JsonConverter(typeof(ConcreteTypeConverter<GitDetails, IGitDetails>))]
	public IGitDetails? Git { get; set; }
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
    public string? Payload { get; set; }
    /**
	   * A counterpart to payload. If payload is free-form Worker input,
		 * then Summary is free-form Worker output. It can optionally be set by a
		 * Worker to provide a summary of the work completed by the Worker and its
		 * Jobs.
	   */
    public string? Summary { get; set; }
    /**
	   * Contains details of the Worker assigned to handle the Event
	   */
    //worker?: worker { get; set; }

    public string? Kind { get; set; }
    public string? ApiVersion { get; set; }
}
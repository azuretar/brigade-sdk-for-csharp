using System.Net;
using Brigade.Meta;
using Brigade.Rest;

namespace Brigade.Core.Events;

public interface IEventsClient
{
    Task<IMetaList<Event>> Create(Event eventItem);
    Task<IListMeta> List(IEventsSelector? selector, IListOptions? opts);
    Task<Event> Get(string id);
    Task<Event> Clone(string id);
    Task UpdateSummary(string id, IEventSummary summary);
    Task<Event> Retry(string id);
    Task Cancel(string id);
    Task<ICancelManyEventsResult> CancelMany(IEventsSelector selector);
    Task Delete(string id);
    Task<IDeleteManyEventsResult> DeleteMany(IEventsSelector selector);
    //IWorkersClient Workers();
    //ILogsClient Logs();
}

public class EventsClient : BaseClient, IEventsClient
{
    public EventsClient(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<IMetaList<Event>> Create(Event eventItem)
    {
        var request = new PostRequest("v2/events", eventItem)
        {
            SuccessCode = HttpStatusCode.Created
        };

        return await Post<MetaList<Event>, Event>(request)!;
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

    public Task<Event> Clone(string id)
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

    public Task<Event> Get(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Event> Retry(string id)
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
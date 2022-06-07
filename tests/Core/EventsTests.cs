using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using Brigade.Core.Events;
using Brigade.Meta;
using Brigade.Shared;
using RichardSzalay.MockHttp;
using Xunit;

namespace tests.core
{
    public class EventsTests
    {
        private readonly EventsClient _client;
        private readonly MockHttpMessageHandler _mockHttp;
        private readonly Fixture _fixture;

        public EventsTests()
        {
            _fixture = new Fixture();

            SetupFixture();

            _mockHttp = new MockHttpMessageHandler();
            var httpClient = _mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("https://localhost/");
            _client = new EventsClient(httpClient);
        }

        private void SetupFixture()
        {
            _fixture.Customizations.Add(
                new TypeRelay(
                    typeof(ISourceState),
                    typeof(SourceState)));

            _fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IObjectMeta),
                    typeof(ObjectMeta)));

            _fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IGitDetails),
                    typeof(GitDetails)));
        }

        [Fact]
        public async Task CreateEvent()
        {
            var testEvent = _fixture.Build<Event>()
                .With(x => x.ApiVersion, Constants.ApiVersion)
                .Create();

            var testEvents = new MetaList<Event>
            {
                Items = new List<Event>
                {
                    _fixture.Create<Event>(),
                    _fixture.Create<Event>()
                }
            };

            var content = await JsonContent.Create(testEvent).ReadAsStringAsync();

            _mockHttp.Expect(HttpMethod.Post, "https://localhost/v2/events")
                .With(new JsonMatcher<Event>(testEvent))
                .Respond(HttpStatusCode.Created, "application/json", JsonSerializer.Serialize(testEvents));

            var result = await _client.Create(testEvent);
            
            Assert.NotNull(result);
            _mockHttp.VerifyNoOutstandingExpectation();
        }

        public async Task ListEvents()
        {
        }
    }
}

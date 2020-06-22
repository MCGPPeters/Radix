using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radix.Chat.Client.Pages;
using static Radix.Option.Extensions;
using SqlStreamStore;

namespace Radix.Chat.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            IStreamStore streamStore = new InMemoryStreamStore();
            SqlStreamStore sqlStreamStore = new SqlStreamStore(streamStore);

            FromEventDescriptor<ChatEvent, Json> fromEventDescriptor = descriptor =>
            {
                if (string.Equals(descriptor.EventType.Value, typeof(ChatEvent).FullName, StringComparison.Ordinal))
                {
                    return Some(JsonSerializer.Deserialize<ChatEvent>(descriptor.Event.Value));
                }

                return None<ChatEvent>();
            };

            IndexViewModel indexViewModel = new IndexViewModel();

            ToTransientEventDescriptor<ChatEvent, Json> toTransientEventDescriptor = (messageId, @event, serialize, eventMetaData, serializeMetaData) =>
                new TransientEventDescriptor<Json>(new EventType(@event.GetType()), serialize(@event), serializeMetaData(eventMetaData), messageId);
            Serialize<ChatEvent, Json> serializeEvent = input =>
            {
                JsonSerializerOptions options = new JsonSerializerOptions { Converters = { new PolymorphicWriteOnlyJsonConverter<ChatEvent>() } };
                string jsonMessage = JsonSerializer.Serialize(input, options);
                return new Json(jsonMessage);
            };
            Serialize<EventMetaData, Json> serializeMetaData = json => new Json(JsonSerializer.Serialize(json));

            GetEventsSince<ChatEvent> getEventsSince = sqlStreamStore.CreateGetEventsSince(
                (json, type) =>
                {
                    if (string.Equals(type.Value, nameof(ChatEvent), StringComparison.Ordinal))
                    {
                        return Some(JsonSerializer.Deserialize<ChatEvent>(json.Value));
                    }

                    return None<ChatEvent>();
                },
                input => Some(JsonSerializer.Deserialize<EventMetaData>(input.Value)));

            BoundedContextSettings<ChatEvent, Json> boundedContextSettings =
                new BoundedContextSettings<ChatEvent, Json>(
                    sqlStreamStore.AppendEvents,
                    getEventsSince,
                    new GarbageCollectionSettings(),
                    fromEventDescriptor,
                    serializeEvent, serializeMetaData);
            BoundedContext<ChatCommand, ChatEvent, Json> boundedContext =
                new BoundedContext<ChatCommand, ChatEvent, Json>(boundedContextSettings);

            builder.Services.AddSingleton(boundedContext);
            builder.Services.AddSingleton(indexViewModel);

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


            

            await builder.Build().RunAsync();
        }
    }
}

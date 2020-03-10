using Radix.Tests.Models;

namespace Radix.Tests
{
    public class EventStoreStub : EventStore<InventoryItemEvent>
    {
        public EventStoreStub(AppendEvents<InventoryItemEvent> appendEvents, GetEventsSince<InventoryItemEvent> getEventsSince)
        {
            AppendEvents = appendEvents;
            GetEventsSince = getEventsSince;
        }

        public AppendEvents<InventoryItemEvent> AppendEvents { get; }
        public GetEventsSince<InventoryItemEvent> GetEventsSince { get; }
    }
}

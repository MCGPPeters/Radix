using System;
using Radix.Validated;
using static Radix.Validated.Extensions;

namespace Radix.Tests.Models
{
    public class CreateInventoryItem : InventoryItemCommand
    {

        private CreateInventoryItem(string name, bool activated, int count)
        {
            Name = name;
            Activated = activated;
            Count = count;
        }

        public string Name { get; }
        public bool Activated { get; }
        public int Count { get; }

        private static Func<string, bool, int, InventoryItemCommand> New => (name, activated, count) =>
            new CreateInventoryItem(name, activated, count);

        public int CompareTo(object obj) => throw new NotImplementedException();

        public int CompareTo(InventoryItemCommand other) => throw new NotImplementedException();

        public bool Equals(InventoryItemCommand other) => throw new NotImplementedException();

        public static Validated<InventoryItemCommand> Create(string? name, bool activated, int count) => Valid(New)
            .Apply(name.IsNotNullNorEmpty("An inventory item must have a name"))
            .Apply(Valid(activated))
            .Apply(
                count > 0
                    ? Valid(count)
                    : Invalid<int>("A new inventory item should have at least 1 instance"));
    }
}

using System;
using System.Threading.Tasks;
using Radix.Blazor.Inventory.Server.Pages;
using Radix.Result;

namespace Radix.Blazor.Inventory.Wasm.Pages
{
    public class Counter : IEquatable<Counter>
    {
        public static Update<Counter, CounterEvent> Update = (state, @event) =>
        {
            state.Count++;
            return state;
        };

        public static Decide<Counter, CounterCommand, CounterEvent> Decide = (state, command) =>
        {
            return Task.FromResult(Extensions.Ok<CounterEvent[], CommandDecisionError>(new[] {new CounterEvent()}));
        };

        public int Count { get; set; }


        public bool Equals(Counter? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Count == other.Count;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Counter)obj);
        }

        public override int GetHashCode() => Count;

        public static bool operator ==(Counter? left, Counter? right) => Equals(left, right);

        public static bool operator !=(Counter? left, Counter? right) => !Equals(left, right);
    }
}

using System;
using Radix.Validated;

namespace Radix
{
    public readonly struct Command<T> : Value<T> where T : IComparable<T>, IEquatable<T>, IComparable
    {
        public static Validated<Command<T>> Create(Func<Validated<T>> create)
        {
            var t = create();
            switch (t)
            {
                case Valid<T>(var valid):
                    return new Valid<Command<T>>(new Command<T>(valid));
                case Invalid<T>(var reasons):
                    return new Invalid<Command<T>>(reasons);
                default:
                    throw new InvalidOperationException();
            }
        }

        public static implicit operator Command<T>(T t)
        {
            return new Command<T>(t);
        }

        public static implicit operator T(Command<T> command)
        {
            return command.Value;
        }

        private Command(T command)
        {
            Value = command;
        }

        public T Value { get; }
    }
}

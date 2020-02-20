namespace Radix.Blazor
{
    public readonly struct Name : Value<string>
    {
        public Name(string v) : this()
        {
            Value = v;
        }

        public string Value { get; }

        public static implicit operator string(Name name)
        {
            return name.Value;
        }

        public static implicit operator Name(string name)
        {
            return new Name(name);
        }
    }


}

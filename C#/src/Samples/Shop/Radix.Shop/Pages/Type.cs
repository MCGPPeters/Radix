namespace Radix.Shop.Pages
{
    public record Type : Alias<string>
    {
        public Type(string Value) : base(Value)
        {
        }

        public static implicit operator string(Type type) => type.Value;
        public static implicit operator Type(string type) => new(type);
    }
}

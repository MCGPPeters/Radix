namespace Radix.Web.Css.Data.Dimensions
{
    public record AbsoluteLength<T> : Length<T>
        where T : Units.Length.Absolute.Unit<T>, Literal<T>
    {
        public AbsoluteLength(Number number)
        {
            Quantity = number;
        }

        public Number Quantity { get; init; }
    }
}

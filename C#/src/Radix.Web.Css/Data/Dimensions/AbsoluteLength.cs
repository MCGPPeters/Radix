namespace Radix.Web.Css.Data.Dimensions
{
    public record AbsoluteLength<T> : Length<T>
        where T : Units.Length.Absolute.Unit<T>, Literal<T>
    {
        public AbsoluteLength(Number number)
        {
            Number = number;
        }

        public Number Number { get; init; }
    }
}

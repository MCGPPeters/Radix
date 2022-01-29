namespace Radix.Web.Css.Data.Dimensions
{
    public record AbsoluteLength<T> : Length
        where T : Units.Length.Absolute.Unit
    {
        public AbsoluteLength(Number number)
        {
            Number = number;
        }

        public Number Number { get; init; }
    }
}

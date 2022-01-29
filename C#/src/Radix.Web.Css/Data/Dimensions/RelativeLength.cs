namespace Radix.Web.Css.Data.Dimensions
{
    public record RelativeLength<T> : Length
        where T : Units.Length.Relative.Unit
    {
        public RelativeLength(Number number)
        {
            Number = number;
        }

        public Number Number { get; init; }
    }
}

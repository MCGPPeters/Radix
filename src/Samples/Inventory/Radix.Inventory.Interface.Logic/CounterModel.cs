namespace Radix.Blazor.Inventory.Interface.Logic;

public record CounterModel
{
    public int Count { get; set; }
    public Error[] Errors { get; set; }
}

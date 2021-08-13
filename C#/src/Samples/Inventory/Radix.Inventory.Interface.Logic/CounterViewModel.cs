using Radix.Components;

namespace Radix.Blazor.Inventory.Interface.Logic;

public record CounterViewModel : ViewModel
{
    public int Count { get; set; }
}

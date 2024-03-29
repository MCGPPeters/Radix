﻿namespace Radix.Blazor.Inventory.Interface.Logic;

public record ItemModel
{
    public required Domain.Data.Aggregate.Address? Id {get;set;}
    public required string Name { get; set; }
    public required bool Activated { get; set; }
}

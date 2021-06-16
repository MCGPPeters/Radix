﻿using System;
using System.Collections.Generic;
using Radix.Components;

namespace Radix.Blazor.Inventory.Interface.Logic
{

    public record IndexViewModel : ViewModel, IEquatable<IndexViewModel>
    {
        public IndexViewModel(List<InventoryItemModel> inventoryItems) => InventoryItems = inventoryItems;

        /// <summary>
        ///     This is just an example.. in real life this would be a database or something
        /// </summary>
        public List<InventoryItemModel>? InventoryItems { get; set; }

        public IEnumerable<Error> Errors { get; set; } = new List<Error>();
    }
}

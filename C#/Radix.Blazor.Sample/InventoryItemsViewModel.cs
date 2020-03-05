using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample
{
    public class InventoryItemsViewModel : ReadModel<InventoryItemsViewModel, InventoryItemEvent>
    {
        private readonly Dictionary<Address, InventoryItemReadModel> inventoryItemReadModels = new Dictionary<Address, InventoryItemReadModel>();
        private IObserver<InventoryItemsViewModel>? _observer;

        public void Apply(InventoryItemEvent @event)
        {
            switch (@event)
            {
                case InventoryItemCreated inventoryItemCreated:
                    var itemReadModel = new InventoryItemReadModel
                        {Activated = inventoryItemCreated.Activated, Name = inventoryItemCreated.Name, Count = inventoryItemCreated.Count};
                    inventoryItemReadModels.Add(@event.Address, itemReadModel);
                    if (_observer is object)
                        _observer.OnNext(this);
                    break;
                case InventoryItemDeactivated _:
                    itemReadModel = inventoryItemReadModels[@event.Address];
                    itemReadModel.Activated = false;
                    if (_observer is object)
                        _observer.OnNext(this);
                    break;
                case ItemsCheckedInToInventory itemsCheckedInToInventory:
                    itemReadModel = inventoryItemReadModels[@event.Address];
                    itemReadModel.Count += itemsCheckedInToInventory.Amount;
                    if (_observer is object)
                        _observer.OnNext(this);
                    break;
                case ItemsRemovedFromInventory itemsRemovedFromInventory:
                    itemReadModel = inventoryItemReadModels[@event.Address];
                    itemReadModel.Count -= itemsRemovedFromInventory.Amount;
                    if (_observer is object)
                        _observer.OnNext(this);
                    break;
                case InventoryItemRenamed inventoryItemRenamed:
                    itemReadModel = inventoryItemReadModels[@event.Address];
                    itemReadModel.Name = inventoryItemRenamed.Name;
                    if (_observer is object)
                        _observer.OnNext(this);
                    break;
                default:
                    throw new NotSupportedException("Unknown event");
            }

        }

        public bool Equals(InventoryItemsViewModel other) =>
            inventoryItemReadModels.SequenceEqual(other.inventoryItemReadModels);

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(InventoryItemEvent value)
        {
            Apply(value);
        }

        public IDisposable Subscribe(IObserver<InventoryItemsViewModel> observer)
        {
            _observer = observer;
            return Disposable.Empty;
        }
    }
}

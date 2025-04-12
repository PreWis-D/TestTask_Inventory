using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class StackingController
{
    private readonly Randomizer _randomizer;
    private readonly int _minCountValue;

    public event Action StackingFailed;
    public event Action<InventoryItem> StackingSuccesed;

    public StackingController(Randomizer randomizer, int minCountValue)
    {
        _randomizer = randomizer;
        _minCountValue = minCountValue;
    }

    public void TryMergeItems(List<InventoryItem> items)
    {
        List<InventoryItem> filterItems = new List<InventoryItem>();
        List<AnimalItem> animalItems = GetAnimalItemList(items);

        for (int i = 0; i < items.Count; i++)
            if (IsAvailable(items[i], items, animalItems))
                filterItems.Add(items[i]);

        var result = filterItems.GroupBy(x => x.Id)
          .Where(g => g.Count() > 1)
          .Select(y => y.Key)
          .ToList();

        if (result.Count < _minCountValue)
            StackingFailed?.Invoke();
        else
            MergeItems(result, filterItems);
    }

    private List<AnimalItem> GetAnimalItemList(List<InventoryItem> items)
    {
        List<AnimalItem> result = new List<AnimalItem>();
        var filter = items.FindAll(x => x.Type == InventoryItemType.Animal);

        for (int i = 0; i < filter.Count; i++)
        {
            var animal = filter[i] as AnimalItem;
            result.Add(animal);
        }

        return result;
    }

    private bool IsAvailable(InventoryItem item, List<InventoryItem> items, List<AnimalItem> animalItems)
    {
        if (CheckFullStack(item.CurrentCount, item.Stack))
            return false;

        if (CheckId(item, items) == false)
            return false;

        var animalItem = item as AnimalItem;
        bool isAvailable = true;

        if (animalItem != null)
            isAvailable = CheckAnimalState(animalItem, animalItems);

        return isAvailable;
    }

    private bool CheckFullStack(int currentValue, int maxValue)
    {
        return currentValue >= maxValue;
    }

    private bool CheckId(InventoryItem item, List<InventoryItem> items)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i] != item && items[i].Id == item.Id)
                return true;

        return false;
    }

    private bool CheckAnimalState(AnimalItem animalItem, List<AnimalItem> animalItems)
    {
        for (int i = 0; i < animalItems.Count; i++)
            if (animalItems[i] != animalItem && animalItems[i].State == animalItem.State && animalItems[i].Id == animalItem.Id)
                return true;

        return false;
    }

    private void MergeItems(List<int> ids, List<InventoryItem> items)
    {
        int random = _randomizer.GetRandomInteger(0, ids.Count);
        List<InventoryItem> filterItems = items.FindAll(x => x.Id == ids[random]);

        InventoryItem itemOne = GetMergeItem(filterItems);
        InventoryItem itemTwo = GetMergeItem(filterItems);

        DistributeValue(itemOne, itemTwo);

        StackingSuccesed?.Invoke(itemTwo);
    }

    private InventoryItem GetMergeItem(List<InventoryItem> items)
    {
        int randomItemOne = _randomizer.GetRandomInteger(0, items.Count);
        InventoryItem item = items[randomItemOne];
        items.Remove(item);
        return item;
    }

    private void DistributeValue(InventoryItem itemOne, InventoryItem itemTwo)
    {
        var sum = itemOne.CurrentCount + itemTwo.CurrentCount;
        var remains = sum - itemOne.Stack;
        var targetValue = remains > 0 ? itemTwo.CurrentCount - remains : itemTwo.CurrentCount;

        itemOne.AddValue(targetValue);
        itemTwo.RemoveValue(targetValue);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class InventoryStorage
{
    private List<InventoryItem> _items = new List<InventoryItem>();
    private List<InventoryItemConfig> _itemConfigs = new List<InventoryItemConfig>();
    private Randomizer _randomizer;

    private int _maxItemsCount;

    private const string _addItemFailedMessage = "Inventory is full!";
    private const string _stackingItemsFailedMessage = "No items for stacking!";
    private const string _removeItemFailedMessage = "Inventory is empty!";
    private const string _cahngeAnimalStateFailedMessage = "There are no animals in the inventory!";
    private const int _minCountItems = 1;

    public event Action InventoryChanged;
    public event Action<InventoryItem> InventoryItemAdded;
    public event Action<string> InventoryChangeFailed;

    [Inject]
    private void Construct(GameConfig gameConfig, Randomizer randomizer)
    {
        _maxItemsCount = gameConfig.InventoryItemConfigsPack.MaxItemsCount;
        _itemConfigs.AddRange(gameConfig.InventoryItemConfigsPack.Configs);
        _randomizer = randomizer;
    }

    public List<InventoryItem> GetInventoryItems()
    {
        return _items;
    }

    #region Add item
    public void TryAddRandomItem()
    {
        if (_items.Count >= _maxItemsCount)
            InventoryChangeFailed?.Invoke(_addItemFailedMessage);
        else
            AddItem(GetRandomConfig(_itemConfigs));
    }

    public void TryAddTargetTypeItem(InventoryItemType type)
    {
        if (_items.Count >= _maxItemsCount)
        {
            InventoryChangeFailed?.Invoke(_addItemFailedMessage);
        }
        else
        {
            var results = _itemConfigs.Where(s => s.ItemType == type);
            List<InventoryItemConfig> itemConfigs = new List<InventoryItemConfig>();
            itemConfigs.AddRange(results);
            AddItem(GetRandomConfig(itemConfigs));
        }
    }

    private void AddItem(InventoryItemConfig config)
    {
        var item = new InventoryItem(config);
        _items.Add(item);
        InventoryChanged?.Invoke();
        InventoryItemAdded?.Invoke(item);
    }

    private InventoryItemConfig GetRandomConfig(List<InventoryItemConfig> itemConfigs)
    {
        return itemConfigs[_randomizer.GetRandomInteger(0, itemConfigs.Count)];
    }
    #endregion

    #region Remove item
    public void TryRemoveRandomItem()
    {
        if (_items.Count < _minCountItems)
            InventoryChangeFailed?.Invoke(_removeItemFailedMessage);
        else
            RemoveItem(GetRandomItem(_items));
    }

    public void TryRemoveLastItem()
    {
        if (_items.Count < _minCountItems)
            InventoryChangeFailed?.Invoke(_removeItemFailedMessage);
        else
            RemoveItem(_items[_items.Count - 1]);
    }

    private void RemoveItem(InventoryItem inventoryItem)
    {
        _items.Remove(inventoryItem);
        InventoryChanged?.Invoke();
    }

    private InventoryItem GetRandomItem(List<InventoryItem> items)
    {
        return items[_randomizer.GetRandomInteger(0, items.Count)];
    }
    #endregion

    #region Animal state
    public void TryChangeAnimalState()
    {
        List<InventoryItem> items = new List<InventoryItem>();

        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Type == InventoryItemType.Animal)
                items.Add(_items[i]);
        }

        if (items.Count < _minCountItems)
            InventoryChangeFailed?.Invoke(_cahngeAnimalStateFailedMessage);
        else
            ChangeAnimalState(GetRandomItem(items));
    }

    private void ChangeAnimalState(InventoryItem inventoryItem)
    {
        // I did not have time for Dedlin ((
    }
    #endregion

    #region Stacking
    public void TryMergeItems()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        items.AddRange(_items);

        for (int i = 0; i < _items.Count; i++)
            if (_items[i].CurrentCount == _items[i].Stack)
                items.Remove(_items[i]);

        var result = items.GroupBy(x => x.Id)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

        if (result.Count < _minCountItems)
            InventoryChangeFailed?.Invoke(_stackingItemsFailedMessage);
        else
            MergeItems(result, items);
    }

    private void MergeItems(List<int> ids, List<InventoryItem> items)
    {
        int random = _randomizer.GetRandomInteger(0, ids.Count);
        List<InventoryItem> tempItems = items.FindAll(x => x.Id == ids[random]);

        int randomItemOne = _randomizer.GetRandomInteger(0, tempItems.Count);
        InventoryItem itemOne = tempItems[randomItemOne];
        tempItems.Remove(itemOne);

        int randomItemTwo = _randomizer.GetRandomInteger(0, tempItems.Count);
        InventoryItem itemTwo = tempItems[randomItemTwo];

        var sum = itemOne.CurrentCount + itemTwo.CurrentCount;
        var remains = sum - itemOne.Stack;
        var targetValue = remains > 0 ? itemTwo.CurrentCount - remains : itemTwo.CurrentCount;

        itemOne.AddValue(targetValue);
        itemTwo.RemoveValue(targetValue);

        if (itemTwo.CurrentCount <= 0)
            RemoveItem(itemTwo);

        InventoryChanged?.Invoke();
    }
    #endregion
}
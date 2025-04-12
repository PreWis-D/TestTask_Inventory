using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Zenject;

public class InventoryStorage
{
    private GameConfig _gameConfig;
    private Randomizer _randomizer;
    private PlayerSaver _playerSaver;

    private List<InventoryItem> _items = new List<InventoryItem>();
    private List<InventoryItemConfig> _itemConfigs = new List<InventoryItemConfig>();
    private StackingController _stackingController;

    private int _maxItemsCount;

    private const string _addItemFailedMessage = "Inventory is full!";
    private const string _stackingItemsFailedMessage = "No items for stacking!";
    private const string _removeItemFailedMessage = "Inventory is empty!";
    private const string _cahngeAnimalStateFailedMessage = "There are no animals in the inventory!";
    private const int _minCountItems = 1;

    public event Action<InventoryItem> InventoryItemRemoved;
    public event Action<InventoryItem> InventoryItemAdded;
    public event Action<InventoryItem> AnimalStateChanged;
    public event Action<string> InventoryChangeFailed;

    [Inject]
    private void Construct(GameConfig gameConfig, Randomizer randomizer, PlayerSaver playerSaver)
    {
        _gameConfig = gameConfig;
        _randomizer = randomizer;
        _playerSaver = playerSaver;
    }

    public void Init()
    {
        _maxItemsCount = _gameConfig.InventoryItemConfigsPack.MaxItemsCount;
        _itemConfigs.AddRange(_gameConfig.InventoryItemConfigsPack.Configs);

        _stackingController = new StackingController(_randomizer, _minCountItems);
        _stackingController.StackingSuccesed += OnStackingSuccesed;
        _stackingController.StackingFailed += OnStackingFailed;

        LoadItems();
    }

    private void LoadItems()
    {
        List<InventoryItem> loadItems = _playerSaver.LoadInventory();

        for (int i = 0; i < loadItems.Count; i++)
        {
            var item = GetInventoryItem(loadItems[i].Type);
            item.Init(loadItems[i].Type, loadItems[i].Name, loadItems[i].Id, loadItems[i].Stack, loadItems[i].CurrentCount, loadItems[i].Icon);
            _items.Add(item);
            InventoryItemAdded?.Invoke(item);
        }
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
            var typeFilter = _itemConfigs.Where(s => s.ItemType == type);
            List<InventoryItemConfig> itemConfigs = new List<InventoryItemConfig>();
            itemConfigs.AddRange(typeFilter);
            AddItem(GetRandomConfig(itemConfigs));
        }
    }

    private void AddItem(InventoryItemConfig config)
    {
        var item = GetInventoryItem(config.ItemType);
        item.Init(config.ItemType, config.Name, config.Id, config.Stack, config.StartValue, config.Icon);
        _items.Add(item);
        InventoryItemAdded?.Invoke(item);

        SaveInventory();
    }

    private InventoryItem GetInventoryItem(InventoryItemType itemType)
    {
        return itemType switch
        {
            InventoryItemType.Animal => new AnimalItem(),
            _ => new InventoryItem(),
        };
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
        InventoryItemRemoved?.Invoke(inventoryItem);

        SaveInventory();
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
            if (_items[i].Type == InventoryItemType.Animal)
                items.Add(_items[i]);

        if (items.Count < _minCountItems)
            InventoryChangeFailed?.Invoke(_cahngeAnimalStateFailedMessage);
        else
            ChangeAnimalState(GetRandomItem(items));
    }

    private void ChangeAnimalState(InventoryItem inventoryItem)
    {
        List<InventoryItem> items = _items.FindAll(x => x.Type == InventoryItemType.Animal);
        int randomIndex = _randomizer.GetRandomInteger(0, items.Count);
        var animalItem = items[randomIndex] as AnimalItem;
        animalItem.ChangeState();
        AnimalStateChanged?.Invoke(animalItem);

        SaveInventory();
    }
    #endregion

    #region Stacking
    public void TryMergeItems()
    {
        _stackingController.TryMergeItems(_items);
    }

    private void OnStackingSuccesed(InventoryItem item)
    {
        if (item.CurrentCount <= 0)
            RemoveItem(item);

        SaveInventory();
    }

    private void OnStackingFailed()
    {
        InventoryChangeFailed?.Invoke(_stackingItemsFailedMessage);
    }
    #endregion

    private void SaveInventory()
    {
        _playerSaver.SaveInventory(_items);
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DebugPanel : BasePanel
{
    [SerializeField] private TMP_Text _debugMessageText;

    [SerializeField] private Button _addRandomItemButton;
    [SerializeField] private Button _addResourcetemButton;
    [SerializeField] private Button _addAnimalItemButton;
    [SerializeField] private Button _addConsumableItemButton;
    [SerializeField] private Button _removeRandomItemButton;
    [SerializeField] private Button _removeLastItemButton;
    [SerializeField] private Button _changeAnimalStateButton;
    [SerializeField] private Button _mergeItemsButton;

    private InventoryStorage _inventoryStorage;

    [Inject]
    private void Construct(InventoryStorage inventoryStorage)
    {
        _inventoryStorage = inventoryStorage;
    }

    public override void Init()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _addRandomItemButton.onClick.AddListener(() => _inventoryStorage.TryAddRandomItem());
        _addResourcetemButton.onClick.AddListener(() => _inventoryStorage.TryAddTargetTypeItem(InventoryItemType.Resource));
        _addAnimalItemButton.onClick.AddListener(() => _inventoryStorage.TryAddTargetTypeItem(InventoryItemType.Animal));
        _addConsumableItemButton.onClick.AddListener(() => _inventoryStorage.TryAddTargetTypeItem(InventoryItemType.Consumable));

        _removeRandomItemButton.onClick.AddListener(() => _inventoryStorage.TryRemoveRandomItem());
        _removeLastItemButton.onClick.AddListener(() => _inventoryStorage.TryRemoveLastItem());

        _changeAnimalStateButton.onClick.AddListener(() => _inventoryStorage.TryChangeAnimalState());

        _mergeItemsButton.onClick.AddListener(() => _inventoryStorage.TryMergeItems());

        _inventoryStorage.InventoryChangeFailed += OnInvenntoryChangeFailed;
        _inventoryStorage.InventoryItemAdded += OnInvenntoryItemAdded;
    }

    private void OnInvenntoryChangeFailed(string message)
    {
        _debugMessageText.color = Color.red;
        _debugMessageText.SetText(message);
    }

    private void OnInvenntoryItemAdded(InventoryItem item)
    {
        _debugMessageText.color = Color.green;
        _debugMessageText.SetText($"Add item: {item.Name}");
    }

    private void Unsubscribe()
    {
        _addAnimalItemButton.onClick.RemoveListener(() => _inventoryStorage.TryAddRandomItem());
        _addResourcetemButton.onClick.RemoveListener(() => _inventoryStorage.TryAddTargetTypeItem(InventoryItemType.Resource));
        _addAnimalItemButton.onClick.RemoveListener(() => _inventoryStorage.TryAddTargetTypeItem(InventoryItemType.Animal));
        _addConsumableItemButton.onClick.RemoveListener(() => _inventoryStorage.TryAddTargetTypeItem(InventoryItemType.Consumable));

        _removeRandomItemButton.onClick.RemoveListener(() => _inventoryStorage.TryRemoveRandomItem());
        _removeLastItemButton.onClick.RemoveListener(() => _inventoryStorage.TryRemoveLastItem());

        _changeAnimalStateButton.onClick.RemoveListener(() => _inventoryStorage.TryChangeAnimalState());

        _mergeItemsButton.onClick.RemoveListener(() => _inventoryStorage.TryMergeItems());

        _inventoryStorage.InventoryChangeFailed -= OnInvenntoryChangeFailed;
        _inventoryStorage.InventoryItemAdded -= OnInvenntoryItemAdded;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}
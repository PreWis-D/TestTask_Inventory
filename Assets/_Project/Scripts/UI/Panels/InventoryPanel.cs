using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InventoryPanel : BasePanel
{
    [SerializeField] private Transform _itemViewsContainer;

    private List<InventoryItemView> _items = new List<InventoryItemView>();

    private InventoryStorage _inventoryStorage;
    private PoolManager _poolManager;
    private InventoryItemView _inventoryItemViewPrefab;

    [Inject]
    private void Construct(GameConfig gameConfig, InventoryStorage inventoryStorage, PoolManager poolManager)
    {
        _inventoryItemViewPrefab = gameConfig.InventoryItemConfigsPack.InventoryItemViewPrefab;
        _inventoryStorage = inventoryStorage;
        _poolManager = poolManager;
    }

    public override void Init()
    {
        _inventoryStorage.InventoryChanged += UpdateInfo;
    }

    public void UpdateInfo()
    {
        var inventoryItems = _inventoryStorage.GetInventoryItems();
        TryRemoveExcess(inventoryItems);

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (_items.Count <= i)
                CreateItem(inventoryItems[i]);
            else
                _items[i].UpdateInfo(inventoryItems[i]);
        }
    }

    private void TryRemoveExcess(List<InventoryItem> inventoryItems)
    {
        var remains = _items.Count - inventoryItems.Count;

        for (int i = 0; i < remains; i++)
        {
            _poolManager.SetPool(_items[i]);
            _items.RemoveAt(i);
        }
    }

    private void CreateItem(InventoryItem inventoryItem)
    {
        var itemView = _poolManager.GetPool(_inventoryItemViewPrefab, Vector3.zero);
        itemView.transform.SetParent(_itemViewsContainer);
        itemView.UpdateInfo(inventoryItem);
        _items.Add(itemView);
    }

    private void OnDestroy()
    {
        _inventoryStorage.InventoryChanged -= UpdateInfo;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryPanel : BasePanel
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Transform _placeHolder;
    [SerializeField] private Transform _itemViewsContainer;
    [SerializeField] private DropZone _dropZone;

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
        _dropZone.Init(_scrollRect, _placeHolder);

        _inventoryStorage.InventoryItemAdded += OnItemAdded;
        _inventoryStorage.InventoryItemRemoved += OnItemRemoved;
        _inventoryStorage.AnimalStateChanged += OnAnimalStateChanged;
    }

    private void OnItemAdded(InventoryItem item)
    {
        AddItem(item);
    }

    private void OnItemRemoved(InventoryItem item)
    {
        RemoveItem(item);
    }

    private void OnAnimalStateChanged(InventoryItem item)
    {
        var itemView = _items.Find(x => x.Item == item);
        itemView.UpdateInfo();
    }

    private void AddItem(InventoryItem inventoryItem)
    {
        var itemView = _poolManager.GetPool(_inventoryItemViewPrefab, Vector3.zero);
        itemView.transform.SetParent(_itemViewsContainer);
        itemView.Init(inventoryItem, _placeHolder, _dropZone.RectTransform, _dropZone.GridLayoutGroup.spacing.x);
        _items.Add(itemView);
        itemView.Spawn();

        UpdateScrollIntarectableState();
    }

    private void RemoveItem(InventoryItem item)
    {
        var itemView = _items.Find(x => x.Item == item);
        _items.Remove(itemView);
        itemView.Despawn(_poolManager);

        UpdateScrollIntarectableState();
    }

    private void UpdateScrollIntarectableState()
    {
        var isIntaractable = _dropZone.RectTransform.rect.height > _scrollRect.GetComponent<RectTransform>().rect.height;
        _scrollRect.enabled = isIntaractable;
    }

    private void OnDestroy()
    {
        _inventoryStorage.InventoryItemAdded -= OnItemAdded;
        _inventoryStorage.InventoryItemRemoved -= OnItemRemoved;
        _inventoryStorage.AnimalStateChanged -= OnAnimalStateChanged;
    }
}
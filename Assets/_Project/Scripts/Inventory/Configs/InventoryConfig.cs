using UnityEngine;

[CreateAssetMenu(fileName = "InventoryConfig", menuName = "Configs/Inventory/InventoryConfig")]
public class InventoryConfig : ScriptableObject
{
    [field: SerializeField] public InventoryItemView InventoryItemViewPrefab { get; private set; }
    [field: SerializeField] public int MaxItemsCount { get; private set; }
    [field: SerializeField] public InventoryItemConfig[] Configs { get; private set; }
}
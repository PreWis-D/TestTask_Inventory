using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemConfig", menuName = "Configs/Inventory/InventoryItemConfig")]
public class InventoryItemConfig : ScriptableObject
{
    [field: SerializeField] public InventoryItemType ItemType { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public int Stack { get; private set; }
    [field: SerializeField] public int StartValue { get; private set; } = 1;
    [field: SerializeField] public Sprite Icon { get; private set; }
}
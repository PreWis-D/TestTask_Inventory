using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [field: SerializeField] public InventoryConfig InventoryItemConfigsPack { get; private set; }
}
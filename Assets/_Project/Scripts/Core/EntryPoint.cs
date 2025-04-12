using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    private GameUIHandler _gameUIHandler;
    private InventoryStorage _inventoryStorage;

    [Inject]
    private void Construct(GameUIHandler gameUIHandler, InventoryStorage inventoryStorage)
    {
        _gameUIHandler = gameUIHandler;
        _inventoryStorage = inventoryStorage;
    }

    private void Start()
    {
        _gameUIHandler.Init();
        _inventoryStorage.Init();
    }
}
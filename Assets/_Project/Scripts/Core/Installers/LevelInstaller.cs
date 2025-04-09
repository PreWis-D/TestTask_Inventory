using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private GameUIHandler _uiHandlerPrefab;

    public override void InstallBindings()
    {
        BindUIHandler();
        BindPoolManager();
        BindInventoryStorage();
        BindRandomizer();
    }

    private void BindUIHandler()
    {
        Container.Bind<GameUIHandler>().FromComponentInNewPrefab(_uiHandlerPrefab).AsSingle();
    }

    private void BindPoolManager()
    {
        Container.Bind<PoolManager>().AsSingle();
    }

    private void BindInventoryStorage()
    {
        Container.Bind<InventoryStorage>().AsSingle();
    }

    private void BindRandomizer()
    {
        Container.Bind<Randomizer>().AsSingle();
    }
}
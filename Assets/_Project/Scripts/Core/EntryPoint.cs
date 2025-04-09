using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour
{
    private GameUIHandler _gameUIHandler;

    [Inject]
    private void Construct(GameUIHandler gameUIHandler)
    {
        _gameUIHandler = gameUIHandler;
    }

    private void Start()
    {
        _gameUIHandler.Init();
    }
}
using UnityEngine;

public class AnimalItem : InventoryItem
{
    public AnimalState State = AnimalState.Healthy;

    public void ChangeState()
    {
        State = GetNewState();
    }

    private AnimalState GetNewState()
    {
        return State switch
        {
            AnimalState.Healthy => AnimalState.Wounded,
            AnimalState.Wounded => AnimalState.Healthy,
            _ => AnimalState.None,
        };
    }
}
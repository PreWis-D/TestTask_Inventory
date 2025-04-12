public class AnimalItem : InventoryItem
{
    public AnimalState State { get; private set; }

    public AnimalItem(InventoryItemConfig config) : base(config)
    {
        State = AnimalState.Healthy;
    }

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
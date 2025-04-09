public class GameUIHandler : BaseUIHandler
{
    public override void Init()
    {
        base.Init();

        GetPanel(PanelType.Inventory).Show();
        GetPanel(PanelType.Debug).Show();
    }
}
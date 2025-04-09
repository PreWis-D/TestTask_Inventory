using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseUIHandler : MonoBehaviour
{
    protected List<IPanel> Panels = new List<IPanel>();

    public virtual void Init()
    {
        Panels.AddRange(GetComponentsInChildren<IPanel>());

        foreach (var panel in Panels)
            panel.Init();

        HideAllPanels();
    }

    protected void HideAllPanels(List<IPanel> extencionPanels = null)
    {
        List<IPanel> panelList = Panels;

        if (extencionPanels != null)
            panelList = GetUniquePanels(extencionPanels);

        foreach (var panel in panelList)
            panel.Hide();
    }

    protected IPanel GetPanel(PanelType type)
    {
        return Panels.First(p => p.Type == type);
    }

    private List<IPanel> GetUniquePanels(List<IPanel> extencionPanels)
    {
        return Panels.Union(extencionPanels).ToList();
    }
}
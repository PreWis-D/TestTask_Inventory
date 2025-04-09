using UnityEngine;

public abstract class BasePanel : MonoBehaviour, IPanel
{
    [SerializeField] private PanelType _type;
    [SerializeField] private Transform _panel;

    public PanelType Type => _type;

    public abstract void Init();

    public virtual void Show()
    {
        _panel.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        _panel.gameObject.SetActive(false);
    }
}
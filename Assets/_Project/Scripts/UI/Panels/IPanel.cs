using System.Collections;
using UnityEngine;

public interface IPanel
{
    public PanelType Type { get; }
    abstract void Init();
    public void Show();
    public void Hide();
}
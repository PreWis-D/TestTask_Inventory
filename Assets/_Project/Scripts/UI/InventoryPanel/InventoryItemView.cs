using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _countText;

    public void UpdateInfo(InventoryItem item)
    {
        _icon.sprite = item.Icon;
        _countText.SetText($"{item.CurrentCount}");
    }
}
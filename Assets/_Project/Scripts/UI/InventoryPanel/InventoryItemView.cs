using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Color _woundedColor = Color.red;

    [Space(10)]
    [Header("Animation settings")]
    [SerializeField] private Transform _viewTransform;
    [SerializeField] private float _animateDuration = 0.25f;

    private Color _healthyColor = Color.white;
    private Tween _tween;

    public InventoryItem Item { get; private set; }

    public void Init(InventoryItem item)
    {
        Item = item;
        _icon.sprite = Item.Icon;

        Item.ValueChanged += UpdateInfo;

        UpdateInfo();
    }

    public void UpdateInfo()
    {
        _countText.SetText($"{Item.CurrentCount}");

        if (Item.Type == InventoryItemType.Animal)
            TryChangeAnimalState(Item);
    }

    public void Spawn()
    {
        _tween.Kill();
        _tween = _viewTransform
            .DOScale(Vector3.one, _animateDuration)
            .From(Vector3.zero)
            .SetEase(Ease.OutBack);
    }

    public void Despawn(PoolManager poolManager)
    {
        _tween.Kill();
        _tween = _viewTransform
            .DOScale(Vector3.zero, _animateDuration)
            .From(Vector3.one)
            .SetEase(Ease.InBack);
        _tween.OnComplete(() =>
        {
            transform.SetParent(null);
            poolManager.SetPool(this);
        });
    }

    private void TryChangeAnimalState(InventoryItem item)
    {
        var animalItem = item as AnimalItem;
        _icon.color = animalItem.State == AnimalState.Wounded ? _woundedColor : _healthyColor;
    }

    private void OnDestroy()
    {
        _tween.Kill();
        Item.ValueChanged -= UpdateInfo;
    }
}
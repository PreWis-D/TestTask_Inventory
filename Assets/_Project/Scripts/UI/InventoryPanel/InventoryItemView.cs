using Coffee.UIExtensions;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Draggable))]
public class InventoryItemView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Color _woundedColor = Color.red;
    [SerializeField] private UIParticle _uiParticle;

    [Space(10)]
    [Header("Animation settings")]
    [SerializeField] private Transform _viewTransform;
    [SerializeField] private Vector3 _mergeScale = new Vector3(1.25f, 1.25f, 1.25f);
    [SerializeField] private float _animateDuration = 0.25f;

    private Draggable _draggable;
    private Color _healthyColor = Color.white;
    private Tween _tween;

    public InventoryItem Item { get; private set; }

    private void Awake()
    {
        _draggable = GetComponent<Draggable>();
    }

    public void Init(InventoryItem item, Transform placeHolder, RectTransform gridRect, float weightSpacing)
    {
        Item = item;
        _icon.sprite = Item.Icon;

        Item.ValueAdded += OnValueAdded;

        _draggable.Init(placeHolder, gridRect, weightSpacing);

        _uiParticle.gameObject.SetActive(false);

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

    private void OnValueAdded()
    {
        UpdateInfo();

        _uiParticle.gameObject.SetActive(false);
        _uiParticle.gameObject.SetActive(true);

        _tween.Kill();
        _tween = _viewTransform
            .DOScale(Vector3.one, _animateDuration)
            .From(_mergeScale)
            .SetEase(Ease.OutBack);
    }

    private void TryChangeAnimalState(InventoryItem item)
    {
        var animalItem = item as AnimalItem;
        _icon.color = animalItem.State == AnimalState.Wounded ? _woundedColor : _healthyColor;
    }

    private void OnDestroy()
    {
        _tween.Kill();
        Item.ValueAdded -= UpdateInfo;
    }
}
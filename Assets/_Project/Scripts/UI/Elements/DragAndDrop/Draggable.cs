using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Transform _placeHolder;
    private CanvasGroup _canvasGroup;
    private float _treshold = 50;
    private float _widhtSpacing;
    private float _widhtContainer;

    public Transform ParentToReturnTo { get; private set; }
    public Transform PlaceHolderParent { get; private set; }

    public void Init(Transform placeHolder, RectTransform gridRect, float widhtSpacing)
    {
        _placeHolder = placeHolder;

        PlaceHolderParent = gridRect;
        ParentToReturnTo = gridRect;
        _widhtContainer = gridRect.rect.width;

        _widhtSpacing = widhtSpacing;

        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetParentToReturnTo(Transform transform)
    {
        ParentToReturnTo = transform;
    }

    public void SetPlaceHolderParent(Transform transform)
    {
        PlaceHolderParent = transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _placeHolder.transform.SetSiblingIndex(transform.GetSiblingIndex());
        var scrollRect = GetComponentInParent<ScrollRect>();
        transform.SetParent(scrollRect.transform);
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;

        if (_placeHolder.transform.parent != PlaceHolderParent)
            _placeHolder.transform.SetParent(PlaceHolderParent);

        int countItemsX = (int)(_widhtContainer / (_rectTransform.sizeDelta.x + _widhtSpacing));

        int countItemsY = (int)(PlaceHolderParent.childCount / countItemsX);
        float countY = (float)PlaceHolderParent.childCount / (float)countItemsX;
        if (countY > countItemsY) 
            countItemsY++;

        int newSiblingIndexX = PlaceHolderParent.childCount;
        UpdateSiblingIndexX(ref newSiblingIndexX, countItemsX);

        int newSiblingIndexY = PlaceHolderParent.childCount;
        UpdateSiblingIndexY(ref newSiblingIndexY);
        
        var resultIndex = newSiblingIndexX + newSiblingIndexY;
        _placeHolder.transform.SetSiblingIndex(resultIndex);
    }

    private void UpdateSiblingIndexX(ref int newSiblingIndexX, int countItemsX)
    {
        for (int i = 0; i < PlaceHolderParent.childCount; i++)
        {
            if (this.transform.position.x < PlaceHolderParent.GetChild(i).position.x + _treshold)
            {
                newSiblingIndexX = i;

                if (_placeHolder.transform.GetSiblingIndex() < newSiblingIndexX)
                    newSiblingIndexX--;

                return;
            }
            else
            {
                newSiblingIndexX = countItemsX - 1;
            }
        }
    }

    private void UpdateSiblingIndexY(ref int newSiblingIndexY)
    {
        for (int i = 0; i < PlaceHolderParent.childCount; i++)
        {
            if (this.transform.position.y > PlaceHolderParent.GetChild(i).position.y - _treshold)
            {
                newSiblingIndexY = i;
                return;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(ParentToReturnTo);
        transform.SetSiblingIndex(_placeHolder.transform.GetSiblingIndex());
        _canvasGroup.blocksRaycasts = true;
        var scrollRect = GetComponentInParent<ScrollRect>();
        _placeHolder.transform.SetParent(scrollRect.transform);
    }
}

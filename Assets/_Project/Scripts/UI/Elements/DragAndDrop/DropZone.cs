using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private ScrollRect _scrollRect;
    private Transform _placeHolder;

    public GridLayoutGroup GridLayoutGroup { get; private set; }
    public RectTransform RectTransform { get; private set; }

    private void Awake()
    {
        GridLayoutGroup = GetComponent<GridLayoutGroup>();
        RectTransform = GetComponent<RectTransform>();
    }

    public void Init(ScrollRect scrollRect, Transform placeHolder)
    {
        _scrollRect = scrollRect;
        _placeHolder = placeHolder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.SetPlaceHolderParent(transform);
            _placeHolder.transform.SetParent(transform);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.PlaceHolderParent == transform)
            d.SetPlaceHolderParent(d.ParentToReturnTo);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.SetParentToReturnTo(transform);
        }
    }
}

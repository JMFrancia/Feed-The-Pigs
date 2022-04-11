using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 * Simple component to make UI draggable
 */
[RequireComponent(typeof(Image))]
public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public delegate void OnDragCallback(PointerEventData data);
    public OnDragCallback OnBeginDrag;
    public OnDragCallback OnDrag;
    public OnDragCallback OnEndDrag;

    public bool Locked
    {
        set
        {
            SetLock(value);
        }
        get
        {
            return _locked;
        }
    }

    bool _locked;
    Image _image;

    void Awake() {
        _image = GetComponent<Image>();
    }

    void SetLock(bool setting) {
        _locked = setting;
        _image.raycastTarget = !_locked;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (OnBeginDrag != null)
        {
            OnBeginDrag(eventData);
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        if (OnDrag != null)
        {
            OnDrag(eventData);
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (OnEndDrag != null)
        {
            OnEndDrag(eventData);
        }
    }
}

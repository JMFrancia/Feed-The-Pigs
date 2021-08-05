using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class FoodItem : MonoBehaviour
{
    public SO_Food Data => _data;

    [SerializeField] SO_Food _data;

    public FoodSlot slot;

    Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        GetComponent<Draggable>().OnEndDrag += OnEndDrag;
    }

    void OnEndDrag(PointerEventData data)
    {
        ReturnToSlot();
    }

    public void ReturnToSlot()
    {
        LeanTween.move(gameObject, slot.transform, .5f).setEase(LeanTweenType.easeOutBounce);
    }

    public void SetData(SO_Food data)
    {
        if (data == null)
            return;
        _data = data;
        _image.sprite = _data.spriteImage;
    }
}

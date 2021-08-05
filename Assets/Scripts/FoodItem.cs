using UnityEngine;
using UnityEngine.UI;

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
    }

    public void SetData(SO_Food data) {
        if (data == null)
            return;
        _data = data;
        _image.sprite = _data.spriteImage;
    }
}

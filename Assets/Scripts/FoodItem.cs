using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FoodItem : MonoBehaviour
{
    [SerializeField] SO_Food _data;

    Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Assign(SO_Food data) {
        if (data == null)
            return;
        _data = data;
        _image.sprite = _data.spriteImage;
    }
}

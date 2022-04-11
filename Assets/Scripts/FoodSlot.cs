using UnityEngine;

public class FoodSlot : MonoBehaviour
{
    [SerializeField] GameObject foodItemPrefab;

    RectTransform _rectTransform;

    const string FOOD_PATH = "Foods/";

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        float width = GetComponent<RectTransform>().rect.width;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, width, width));
    }

    public FoodItem GenerateFoodItem(FoodType type) {
        FoodItem item = GameObject.Instantiate(foodItemPrefab, transform).GetComponent<FoodItem>();
        item.SetData(Resources.Load<SO_Food>($"{FOOD_PATH}{type}"));
        AssignItem(item);
        return item;
    }

    public void AssignItem(FoodItem item, bool animation = false) {
        item.GetComponent<RectTransform>().sizeDelta = new Vector2(_rectTransform.rect.width, _rectTransform.rect.width);
        if (animation)
        {
            LeanTween.move(item.gameObject, transform, .2f).setEase(LeanTweenType.notUsed);
        }
        else
        {
            item.transform.position = transform.position;
        }
        item.slot = this;
    }
}

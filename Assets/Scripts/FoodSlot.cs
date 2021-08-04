using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSlot : MonoBehaviour
{
    [SerializeField] GameObject foodItemPrefab;

    FoodItem _item;
    RectTransform _rectTransform;

    const string _foodPath = "Foods/";

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

    public void GenerateFoodItem(FoodType type) {
        FoodItem item = GameObject.Instantiate(foodItemPrefab, transform).GetComponent<FoodItem>();
        item.Assign(Resources.Load<SO_Food>($"{_foodPath}{type}"));
        AssignItem(item);
    }

    void AssignItem(FoodItem item) {
        _item = item;
        _item.GetComponent<RectTransform>().sizeDelta = new Vector2(_rectTransform.rect.width, _rectTransform.rect.width);
        _item.transform.position = transform.position;
    }

}

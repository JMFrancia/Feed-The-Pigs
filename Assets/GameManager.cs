using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using DentedPixel;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _foodSlotGrid;
    [SerializeField] DropZone _dropZone;
    [SerializeField] FoodSlot[] _dropSlots;

    FoodSlot[] _foodSlots;
    int correctItems = 0;

    private void Awake()
    {
        _foodSlots = _foodSlotGrid.GetComponentsInChildren<FoodSlot>();
        _dropZone.OnDrop += OnDrop;
    }

    void OnDrop(GameObject obj, PointerEventData data) {
        FoodItem item = data.pointerDrag.GetComponent<FoodItem>();
        Debug.Log(item.Data.name + " dropped on " + obj.name);

        if (correctItems < 3) {
            if (item.Data.produce)
            {
                CorrectItem(item);
            }
            else {
                WrongItem(item);
            }
        }
    }

    void CorrectItem(FoodItem item)
    {
        Debug.Log("Correct!");
        _dropSlots[correctItems++].AssignItem(item, true);
        item.GetComponent<Draggable>().Locked = true;
    }

    void WrongItem(FoodItem item) {
        Debug.Log("Wrong!");
        LeanTween.move(item.gameObject, item.slot.transform, .5f).setEase(LeanTweenType.easeOutBounce);
    }

    private void Start()
    {
        List<FoodType> shuffledTypes = GetShuffledFoodTypes();
        for (int n = 0; n < _foodSlots.Length; n++) {
            _foodSlots[n].GenerateFoodItem(shuffledTypes[n % shuffledTypes.Count]);
        }
    }

    List<FoodType> GetShuffledFoodTypes() {
        List<FoodType> allTypes = Enum.GetValues(typeof(FoodType)).Cast<FoodType>().ToList();
        System.Random r = new System.Random();
        return allTypes.OrderBy(x => r.Next()).ToList();
    }

    FoodType GetRandomFoodType() {
        Array allTypes = Enum.GetValues(typeof(FoodType));
        return (FoodType) allTypes.GetValue(UnityEngine.Random.Range(0, allTypes.Length));
    }
}

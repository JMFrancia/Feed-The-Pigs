using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _foodSlotGrid;

    FoodSlot[] _foodSlots;

    private void Awake()
    {
        _foodSlots = _foodSlotGrid.GetComponentsInChildren<FoodSlot>();
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

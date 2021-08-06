using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using DentedPixel;

[RequireComponent(typeof(StateManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _foodSlotGrid;
    [SerializeField] DropZone _dropZone;
    [SerializeField] FoodSlot[] _dropSlots;

    FoodSlot[] _foodSlots;
    int _correctItems = 0;
    StateManager _stateManager;

    private void Awake()
    {
        _stateManager = GetComponent<StateManager>();
        _foodSlots = _foodSlotGrid.GetComponentsInChildren<FoodSlot>();
        _dropZone.OnDrop += OnDrop;
        Initialize();
    }



    void Initialize()
    {
        _stateManager.AddState(Constants.GameStates.INTRO);
        _stateManager.AddState(Constants.GameStates.REQUEST);
        _stateManager.AddState(Constants.GameStates.PLAY);
        _stateManager.AddState(Constants.GameStates.SUCCESS);
        _stateManager.AddTransition(Constants.GameStates.INTRO, Constants.GameStates.REQUEST);
        _stateManager.AddTransition(Constants.GameStates.REQUEST, Constants.GameStates.PLAY);
        _stateManager.AddTransition(Constants.GameStates.PLAY, Constants.GameStates.SUCCESS);
        _stateManager.AddTransition(Constants.GameStates.SUCCESS, Constants.GameStates.INTRO);
    }

    void OnDrop(GameObject obj, PointerEventData data) {
        FoodItem item = data.pointerDrag.GetComponent<FoodItem>();

        if (_correctItems < 3) {
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
        _dropSlots[_correctItems++].AssignItem(item, true);
        item.GetComponent<Draggable>().Locked = true;
    }

    void WrongItem(FoodItem item) {
        Debug.Log("Wrong!");
        item.Dispose();
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

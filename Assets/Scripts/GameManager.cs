using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;

[RequireComponent(typeof(StateManager))]
[RequireComponent(typeof(ConvoManager))]
public class GameManager : MonoBehaviour
{

    [SerializeField] bool skipIntro = false;
    [SerializeField] GameObject _foodSlotGrid;
    [SerializeField] DropZone _dropZone;
    [SerializeField] FoodSlot[] _dropSlots;
    [SerializeField] TextMeshProUGUI requestText;

    FoodSlot[] _foodSlots;
    int _correctItems = 0;
    StateManager _stateManager;
    List<RequestCategory> _categories;
    List<FoodItem> _items;
    int _categoryIndex = 0;
    RequestCategory _currentCategory => _categories[_categoryIndex];
    CircularQueue<FoodType> _foodTypes;
    ConvoManager _convoManager;

    Dictionary<RequestCategory, FoodType> _specificWrongItems;

    private void Awake()
    {
        _stateManager = GetComponent<StateManager>();
        _convoManager = GetComponent<ConvoManager>();
        _foodSlots = _foodSlotGrid.GetComponentsInChildren<FoodSlot>();
        _dropZone.OnDrop += OnDrop;
    }

    private void Start()
    {
        InitializeFoodTypes();
        _categories = Enum.GetValues(typeof(RequestCategory)).Cast<RequestCategory>().OrderBy(x => Guid.NewGuid()).ToList();
        InitializeStates();

        _specificWrongItems = new Dictionary<RequestCategory, FoodType>() {
            { RequestCategory.Dessert, FoodType.Carrot },
            { RequestCategory.Raw, FoodType.Cake },
            { RequestCategory.Vegetarian, FoodType.Chicken },
            { RequestCategory.Junk, FoodType.Grapes },
            { RequestCategory.Healthy, FoodType.Frenchfries }
        };
    }

    void InitializeFoodTypes()
    {
        List<FoodType> allTypes = Enum.GetValues(typeof(FoodType)).Cast<FoodType>().ToList();
        _foodTypes = new CircularQueue<FoodType>(allTypes);
        _foodTypes.Shuffle();
    }

    void GenerateFoods()
    {
        _items = new List<FoodItem>();
        _foodTypes.Shuffle();
        for (int n = 0; n < _foodSlots.Length; n++)
        {
            _items.Add(_foodSlots[n].GenerateFoodItem(_foodTypes.Next()));
        }
    }

    void ClearFoods() {
        for (int n = 0; n < _items.Count; n++) {
            _items[n].Dispose();
        }
        _items.Clear();
    }

    void InitializeStates()
    {
        _stateManager.CreateState(Constants.GameStates.INTRO, onStart: OnEnterIntro, waitForStart: true);
        _stateManager.CreateState(Constants.GameStates.REQUEST, onStart: OnEnterRequest);
        _stateManager.CreateState(Constants.GameStates.PLAY, onStart: OnEnterPlay);
        _stateManager.CreateState(Constants.GameStates.SUCCESS, onStart: OnEnterSuccess);
        _stateManager.AddTransition(Constants.GameStates.INTRO, Constants.GameStates.REQUEST);
        _stateManager.AddTransition(Constants.GameStates.REQUEST, Constants.GameStates.PLAY);
        _stateManager.AddTransition(Constants.GameStates.PLAY, Constants.GameStates.SUCCESS);
        _stateManager.AddTransition(Constants.GameStates.SUCCESS, Constants.GameStates.REQUEST);
        if (skipIntro)
        {
            _stateManager.Initialize(Constants.GameStates.REQUEST);

        }
        else
        {
            _stateManager.Initialize(Constants.GameStates.INTRO);
        }
    }

    void OnEnterIntro() {
        //Play boar intro
        //Bring in plate UI
        //OnComplete: Transition to Request
        Debug.Log("Entering intro");
        _convoManager.PlayGameIntro(() => {
            _stateManager.CompleteTransition();
            _stateManager.Transition(Constants.GameStates.REQUEST);
        });
    }

    void OnEnterRequest() {
        //Play boar request. On complete, Bring in food UI. On Complete, transition to Play
        Debug.Log("Entering request");
        _categoryIndex = (_categoryIndex + 1) % _categories.Count;
        _convoManager.PlayCategoryIntro(_currentCategory, () => {
            SetRequestText(_currentCategory);
            _stateManager.CompleteTransition();
            _stateManager.Transition(Constants.GameStates.PLAY);
        }); 
    }

    void OnEnterPlay() {
        Debug.Log("Entering play");
        GenerateFoods();
        //Begin timer sequence for idle SFX
        //Unlock Food UI?
    }

    void OnEnterSuccess() {
        Debug.Log("Entering success");
        ClearFoods();
        //Success SFX
        //On complete, transition to request
        _stateManager.Transition(Constants.GameStates.REQUEST);
    }

    void SetRequestText(RequestCategory category) {
        requestText.text = Constants.Requests[category];
        //Play SFX for request
    }

    void OnDrop(GameObject obj, PointerEventData data) {
        FoodItem item = data.pointerDrag.GetComponent<FoodItem>();
        if (ItemMatchesCategory(item, _currentCategory))
        {
            CorrectItem(item);
        }
        else {
            WrongItem(item);
        }
    }

    bool ItemMatchesCategory(FoodItem item, RequestCategory category) {
        bool result = false;
        switch (category) {
            case RequestCategory.Dessert:
                result = item.Data.dessert;
                break;
            case RequestCategory.Healthy:
                result = item.Data.healthy;
                break;
            case RequestCategory.Junk:
                result = item.Data.junk;
                break;
            case RequestCategory.Vegetarian:
                result = item.Data.vegetarian;
                break;
            case RequestCategory.Raw:
                result = item.Data.raw;
                break;
        }
        return result;
    }

    void CorrectItem(FoodItem item)
    {
        Debug.Log("Correct!");
        _dropSlots[_correctItems++].AssignItem(item, true);
        item.GetComponent<Draggable>().Locked = true;
        Debug.Log("Correct: " + _correctItems);
        if (_correctItems == 3)
        {
            _convoManager.PlayCorrect(() =>
            {
                _stateManager.CompleteTransition();
                _stateManager.Transition(Constants.GameStates.SUCCESS);
            });
            _correctItems = 0;
            //Play correct SFX
            //Lock UI until complete
            //OnComplete; Transition to Success

        }
        else {
            _convoManager.PlayCorrect();
            //Play correct SFX
            //Lock UI until complete
        }
    }

    void WrongItem(FoodItem item)
    {
        if (item.Data.jokeItem)
        {
            _convoManager.PlayJokeChoice(item.Data.type);
        }
        else if (item.Data.type == _specificWrongItems[_currentCategory])
        {
            _convoManager.PlaySpecificWrong(_currentCategory);
        }
        else
        {
            _convoManager.PlayWrong();
        }
        Debug.Log("Wrong!");
        _items.Remove(item);
        item.Dispose();
    }

    


}

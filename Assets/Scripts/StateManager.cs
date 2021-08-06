using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StaticUpdate))]
public class StateManager : MonoBehaviour
{
    public GameState Current => _current;
    public Dictionary<string, GameState> States => _states;

    Dictionary<string, GameState> _states = new Dictionary<string, GameState>();
    Dictionary<GameState, Dictionary<string, GameState>> _transitions = new Dictionary<GameState, Dictionary<string, GameState>>();
    GameState _current;

    private void Awake()
    {
        StaticUpdate.OnUpdate += Update;        
    }

    //Via static update
    private void Update()
    {
        _current?.OnUpdate?.Invoke();
    }

    public void Initialize(string name, bool callOnStart = true) {
        if (!_states.ContainsKey(name)) {
            Debug.LogError($"Attempting to initialize non-existant state {name}");
            return;
        }
        _current = _states[name];
        if (callOnStart)
        {
            _current.OnStart?.Invoke();
        }
    }

    public void AddState(GameState state) {
        _states.Add(state.Name, state);
    }

    public GameState CreateState(string name, System.Action onStart = null, System.Action onUpdate = null, System.Action onExit = null) {
        GameState result = new GameState(name, onStart, onUpdate, onExit);
        AddState(result);
        return result;
    }

    public void AddTransition(GameState state1, GameState state2) {
        if (!_transitions.ContainsKey(state1)) {
            _transitions[state1].Add(state2.Name, state2);
        }
        _transitions.Add(state1, new Dictionary<string, GameState>());
    }

    public void AddTransition(string name1, string name2)
    {
        if (!_states.ContainsKey(name1)) {
            Debug.LogError("State \"{name1}\" does not exist");
            return;
        }
        if (!_states.ContainsKey(name2))
        {
            Debug.LogError("State \"{name2}\" does not exist");
            return;
        }
        AddTransition(_states[name1], _states[name2]);
    }

    public void Transition(string name) {
        if (!_transitions[_current].ContainsKey(name))
        {
            Debug.LogError($"Illegally trying to transition from state {name} to state {name}; no such transition exists");
            return;
        }
        _current.OnExit?.Invoke();
        _current = _transitions[_current][name];
        _current.OnStart?.Invoke();
    }
}

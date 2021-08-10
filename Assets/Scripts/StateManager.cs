using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public GameState Current => _current;
    public Dictionary<string, GameState> States => _states;

    public static bool Waiting = false;

    Dictionary<string, GameState> _states = new Dictionary<string, GameState>();
    Dictionary<GameState, Dictionary<string, GameState>> _transitions = new Dictionary<GameState, Dictionary<string, GameState>>();
    GameState _current;
    GameState _next;
    bool _transitionNextFrame = false;
    bool _startRoutine = false;
    bool _exitRoutine = false;

    private void Update()
    {
        if (_transitionNextFrame) {
            _current = _next;
            _next = null;
            _transitionNextFrame = false;

            _current.OnStart?.Invoke();
            if (_current.waitForStartToComplete) {
                WaitForOnStartToFinish();
            }
            return;
        }

        bool dontUpdate = (_startRoutine && !_current.callUpdateDuringStart) ||
                            (_exitRoutine && !_current.callUpdateDuringExit);
        if (!dontUpdate)
        {
            _current?.OnUpdate?.Invoke();
        }
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
            if (_current.waitForStartToComplete) {
                WaitForOnStartToFinish();
            }
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
            _transitions.Add(state1, new Dictionary<string, GameState>());
        }
        _transitions[state1].Add(state2.Name, state2);
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
            Debug.LogError($"Illegally trying to transition from state {_current.Name} to state {name}; no such transition exists");
            return;
        }
        _current.OnExit?.Invoke();
        if (_current.waitForExitToComplete)
        {
            WaitForOnExitToFinish(name);
        }
        else
        {
            TransitionOnNextFrame(name);
        }
    }

    void WaitForOnStartToFinish() {
        _startRoutine = true;
        StartCoroutine(WaitForCallbackToFinish(false));
    }

    void WaitForOnExitToFinish(string nextStateName) {
        _exitRoutine = true;
        StartCoroutine(WaitForCallbackToFinish(true, nextStateName));
    }

    IEnumerator WaitForCallbackToFinish(bool transitionOnComplete, string nextStateName = null) {
        Waiting = true;
        while (Waiting) {
            yield return null;
        }
        _startRoutine = false;
        _exitRoutine = false;
        if (transitionOnComplete)
        {
            TransitionOnNextFrame(name);
        }
    }

    void TransitionOnNextFrame(string name) {
        _next = _transitions[_current][name];
        _transitionNextFrame = true;
    }
}


/*
 * Works in conjunction with game state manager to have
 * simple FSM with helpful callbacks
 */
public class GameState
{
    public System.Action OnStart;
    public System.Action OnUpdate;
    public System.Action OnExit;

    public bool waitForStartToComplete = false;
    public bool callUpdateDuringStart = false;
    public bool waitForExitToComplete = false;
    public bool callUpdateDuringExit = false;

    public string Name => _name;

    string _name;

    public GameState(string name, System.Action onStart = null, System.Action onUpdate = null, System.Action onExit = null, bool waitForStart = false, bool callUpdateDuringStart = true, bool waitForExit = false, bool callUpdateDuringExit = true) {
        this._name = name;
        OnStart = onStart;
        OnUpdate = onUpdate;
        OnExit = onExit;
        waitForExitToComplete = waitForStart;
        this.callUpdateDuringStart = callUpdateDuringStart;
        waitForExitToComplete = waitForExit;
        this.callUpdateDuringExit = callUpdateDuringExit;
    }


}

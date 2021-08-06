public class GameState
{
    public System.Action OnStart;
    public System.Action OnUpdate;
    public System.Action OnExit;

    public string Name => _name;

    string _name;

    public GameState(string name) {
        this._name = name;
    }
}

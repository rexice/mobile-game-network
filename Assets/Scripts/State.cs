public interface State {
    void Start() { }

    void Tick(Enemy e);

    State Transition(Enemy e) { return null; }

    void Exit() { }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactToPlayer : State
{
    public void Tick(Enemy e) {
        Debug.Log("reacting to player");
    }

    public State Transition(Enemy e) {
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AddCallbacks : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var ui = GetComponent<UIDocument>();
        var button = ui.rootVisualElement.Q<Button>("Fight");
        button.clicked += BattleManager.Fight;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

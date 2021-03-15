using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelControllerBrain : MonoBehaviour
{
    
    private Subscription<AddWinConEvent> addWinConSubscription;
    private Condition winCon;

    private bool winConMet = false;

    // Start is called before the first frame update
    void Awake()
    {
        addWinConSubscription = EventBus.Subscribe<AddWinConEvent>(AddWinCon);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!winConMet && CheckWinCon()) {
            winConMet = true;
            EventBus.Publish<LevelClearEvent>(new LevelClearEvent());
        }
    }

    private void AddWinCon(AddWinConEvent _event)
    {
        winCon += _event.condition;
    }

    private bool CheckWinCon()
    {
        // If there is no win condition, then return false
        if (winCon == null) return false;


        // Evaluate all win conditions
        var results = winCon.GetInvocationList().Select(
            x => (bool)x.DynamicInvoke()
        );

        // Concatenate results
        bool finalResult = true;
        foreach (bool result in results) {
            finalResult = finalResult && result;
        }

        // Return final results
        return finalResult;
    }

}

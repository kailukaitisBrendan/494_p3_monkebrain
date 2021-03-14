using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Includes a list of Event class declarations / constructors
*/

// Empty Event Class (deprecated, since it's not descriptive)
public class Event
{
}


// USE: published when a win condition should be added
public class AddWinConEvent
{
    public Condition condition;

    public AddWinConEvent(Condition _condition)
    {
        condition = _condition;
    }
}

public class LevelClearEvent
{
    public LevelClearEvent()
    {
    }
}


// USE: published when a button is pressed
public class ButtonPressEvent
{
    public int id;

    public ButtonPressEvent(int _id)
    {
        id = _id;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Includes a list of Event class declarations / constructors
*/


// USE: (deprecated, since it's not descriptive)
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


// USE: published when a level is cleared
public class LevelClearEvent
{
    public LevelClearEvent()
    {
    }
}


// USE: published when a level is failed
public class LevelFailEvent
{
    public LevelFailEvent()
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


// USE: published when a button is lifted
public class ButtonLiftEvent
{
    public int id;
    public ButtonLiftEvent(int _id)
    {
        id = _id;
    }
}

// USE: published when enemy spots player
public class PlayerSpottedEvent
{
    public GameObject player;
    public GameObject enemy;
    public PlayerSpottedEvent(GameObject _player, GameObject _enemy)
    {
        player = _player;
        enemy = _enemy;
    }

}

public class HitObjectEvent
{
    public Vector3 position;

    public HitObjectEvent(Vector3 _position)
    {
        position = _position;
    }
}

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
    public GameObject sourceObject;
    public GameObject hitObject;

    public HitObjectEvent(GameObject _sourceObject, GameObject _hitObject)
    {
        sourceObject = _sourceObject;
        hitObject = _hitObject;
    }
}

public class MovementEvent
{
    public bool isWalking;
    public bool isJumping;
    public bool isGrounded;

    public MovementEvent(bool _isWalking, bool _isJumping, bool _isGrounded)
    {
        isWalking = _isWalking;
        isJumping = _isJumping;
        isGrounded = _isGrounded;
    }
}

public class EnemyStateEvent
{
    public bool isWalking;
    public bool isDistracted;
    public bool isDazed;
    public bool drawingGun;

    public EnemyStateEvent(bool _isWalking, bool _isDrawingGun, bool _isAlerted, bool _drawingGun)
    {
        isWalking = _isWalking;
        isDistracted = _isDrawingGun;
        isDazed = _isAlerted;
        drawingGun = _drawingGun;
    }
}

public class ObjectInteractionEvent
{
    public ObjectInteractionEvent()
    {
        
    }
}

public class ThrowingEvent
{
    public ThrowingEvent()
    {
        
    }
}
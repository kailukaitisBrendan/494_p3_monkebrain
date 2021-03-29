﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyMovementController : MonoBehaviour
{

    public DesiredPositionIsGameobject desiredPositionIsGameobject;
    public FieldOfView fieldOfView;

    public GameObject[] pathPoints;
    int pointIndex = 0;

    public float chaseSpeed = 6f;
    public float dazeTime = 7f;
    public bool isStatic;
    float normalSpeed;
    bool isChasingPlayer = false;
    bool isDistracted = false;
    bool isDazed = false;

    public TextMeshPro emoteText;

    Subscription<PlayerSpottedEvent> playerSpottedSubscription;

    Subscription<HitObjectEvent> hitObjectSubscription;


    // Start is called before the first frame update
    void Start()
    {
        playerSpottedSubscription = EventBus.Subscribe<PlayerSpottedEvent>(_OnPlayerSpotted);
        hitObjectSubscription = EventBus.Subscribe<HitObjectEvent>(_OnHitObject);

        desiredPositionIsGameobject = GetComponent<DesiredPositionIsGameobject>();
        fieldOfView = GetComponent<FieldOfView>();

        normalSpeed = desiredPositionIsGameobject.agent.speed;

        if (pathPoints.Length < 1)
            return;

        // set initial enemy spot to intial point and increment index
        transform.position = pathPoints[pointIndex].transform.position;
        pointIndex = (pointIndex + 1) % pathPoints.Length;

        // start enemy movement
        StartCoroutine(MoveEnemy());
    }

    IEnumerator MoveEnemy()
    {
        while (true)
        {
            GameObject target = pathPoints[pointIndex];

            // wait until enemy has reached the target
            yield return StartCoroutine(WaitToGetToPoint(target));

            // increment pointIndex
            pointIndex = (pointIndex + 1) % pathPoints.Length;
        }
    }

    IEnumerator WaitToGetToPoint(GameObject target)
    {
        // set desired target
        desiredPositionIsGameobject.target = target;

        while (!almostEqual(transform.position.x, target.transform.position.x, .1f) || !almostEqual(transform.position.z, target.transform.position.z, .1f))
        {
            yield return null;
        }

        desiredPositionIsGameobject.agent.ResetPath();

        yield return null;
    }

    // Collisions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player") && !isDazed)
        {
            // Player is caught by enemy. Stop all movement and have player "die."
            StopAllCoroutines();
            desiredPositionIsGameobject.StopAllCoroutines();
            PlayerMovement playerMovement = collision.collider.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }
            StartCoroutine(WaitToDie());
        }
    }

    IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(1f);
        EventBus.Publish<LevelFailEvent>(new LevelFailEvent());
    }

    // Events

    void _OnPlayerSpotted(PlayerSpottedEvent e)
    {
        // check to see if it is the right enemy
        if (e.enemy != this.gameObject)
            return;

        // change target to be the player
        if (e.player)
        {
            StopAllCoroutines();
            Debug.Log("Player Spotted!: " + e.player.transform.position);
            desiredPositionIsGameobject.agent.ResetPath();

            desiredPositionIsGameobject.target = e.player;
            isChasingPlayer = true;
            fieldOfView.cur_view_range = 360f;
            emoteText.text = "!";

            desiredPositionIsGameobject.agent.speed = chaseSpeed;
        }
        // change target to move to the waypoints
        else
        {
            desiredPositionIsGameobject.agent.ResetPath();

            desiredPositionIsGameobject.target = null;
            isChasingPlayer = false;
            fieldOfView.cur_view_range = fieldOfView.view_range;
            emoteText.text = "";

            desiredPositionIsGameobject.agent.speed = normalSpeed;
            if (pathPoints.Length < 1)
                return;

            StartCoroutine(MoveEnemy());
        }
    }

    void _OnHitObject(HitObjectEvent e)
    {
        Debug.Log(e.hitObject); // ground
        Debug.Log(e.sourceObject); // golden package

        bool packageInRange = fieldOfView.PackageInFieldOfView(e.sourceObject.transform.position);
        Debug.Log("packageinrange? " + packageInRange);

        bool enemyOccupied = isChasingPlayer || isDistracted || isDazed;

        if (packageInRange && !enemyOccupied)
        {
            if (e.hitObject.CompareTag("Enemy"))
            {
                // make sure it is this enemy
                if (e.hitObject != this.gameObject)
                    return;

                Debug.Log("Daze Enemy");
                StartCoroutine(DazeEnemy());
            }
            else if(!isStatic)
            {
                Debug.Log("Distract Enemy");
                StartCoroutine(DistractEnemy(e.sourceObject));
            }
        }

    }

    IEnumerator DistractEnemy(GameObject box)
    {
        isDistracted = true;
        emoteText.text = "?";

        Debug.Log("Chase Box at: " + box.transform.position);
        desiredPositionIsGameobject.target = box;

        desiredPositionIsGameobject.agent.ResetPath();

        yield return StartCoroutine(WaitToGetToPoint(box));

        EventBus.Publish<PlayerSpottedEvent>(new PlayerSpottedEvent(null, transform.gameObject));
        isDistracted = false;
        emoteText.text = "";
    }

    IEnumerator DazeEnemy()
    {
        isDazed = true;
        emoteText.text = "*";
        desiredPositionIsGameobject.target = null;

        desiredPositionIsGameobject.agent.ResetPath();

        yield return new WaitForSeconds(dazeTime);
        EventBus.Publish<PlayerSpottedEvent>(new PlayerSpottedEvent(null, transform.gameObject));
        isDazed = false;
        emoteText.text = "";
    }


    public bool almostEqual(float a, float b, float eps)
    {
        return Mathf.Abs(a - b) < eps;
    }
}

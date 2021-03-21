using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{

    public DesiredPositionIsGameobject desiredPositionIsGameobject;

    public GameObject[] pathPoints;
    int pointIndex = 0;
    bool caughtPlayer = false;

    public float chaseSpeed = 6f;
    float normalSpeed;


    Subscription<PlayerSpottedEvent> playerSpottedSubscription;

    // Start is called before the first frame update
    void Start()
    {
        playerSpottedSubscription = EventBus.Subscribe<PlayerSpottedEvent>(_OnPlayerSpotted);

        desiredPositionIsGameobject = GetComponent<DesiredPositionIsGameobject>();
        normalSpeed = desiredPositionIsGameobject.agent.speed;

        if (pathPoints.Length < 1)
            return;

        // set initial enemy spot to intial point
        transform.position = pathPoints[pointIndex].transform.position;
        pointIndex = (pointIndex + 1) % pathPoints.Length;

        StartCoroutine(MoveEnemy());
    }

    IEnumerator MoveEnemy()
    {
        while (true)
        {
            GameObject target = pathPoints[pointIndex];
            //target.y = transform.position.y;

            yield return StartCoroutine(WaitToGetToPoint(target));

            // increment pointIndex
            pointIndex = (pointIndex + 1) % pathPoints.Length;
        }
    }

    IEnumerator WaitToGetToPoint(GameObject target)
    {
        desiredPositionIsGameobject.target = target;

        while (transform.position.x != target.transform.position.x || transform.position.z != target.transform.position.z)
        {
            yield return null;
        }
        desiredPositionIsGameobject.agent.ResetPath();

        yield return null;
    }

    void _OnPlayerSpotted(PlayerSpottedEvent e)
    {
        // check to see if it is the right enemy
        if (e.enemy != this.gameObject)
            return;

        // change target to be either the list of waypoints or the player
        if (e.player)
        {
            StopAllCoroutines();
            Debug.Log("Player Spotted!: " + e.player.transform.position);
            desiredPositionIsGameobject.agent.ResetPath();

            desiredPositionIsGameobject.target = e.player;

            StartCoroutine(desiredPositionIsGameobject.PathfindingLoop());
            desiredPositionIsGameobject.agent.speed = chaseSpeed;
        }
        else
        {
            desiredPositionIsGameobject.agent.ResetPath();

            desiredPositionIsGameobject.target = null;
            desiredPositionIsGameobject.agent.speed = normalSpeed;
            if (pathPoints.Length < 1)
                return;

            StartCoroutine(MoveEnemy());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            PlayerMovement playerMovement = collision.collider.gameObject.GetComponent<PlayerMovement>();
            //if (playerMovement != null)
            //{
            //    playerMovement.enabled = false;
            //}
            StartCoroutine(WaitToDie());
        }
    }

    IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(1f);
        EventBus.Publish<LevelFailEvent>(new LevelFailEvent());
    }
}
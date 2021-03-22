using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public GameObject[] pathPoints;
    public float enemySpeed = 3f;
    public float turnSpeed = .1f;
    int pointIndex = 0;
    bool caughtPlayer = false;

    Subscription<PlayerSpottedEvent> playerSpottedSubscription;

    // Start is called before the first frame update
    void Start()
    {
        playerSpottedSubscription = EventBus.Subscribe<PlayerSpottedEvent>(_OnPlayerSpotted);

        if (pathPoints.Length <= 1)
            return;

        // set initial enemy spot to intial point
        transform.position = pathPoints[pointIndex].transform.position;

        pointIndex = (pointIndex + 1) % pathPoints.Length;

        MoveEnemy();
    }

    void MoveEnemy()
    {
        StartCoroutine(MoveEnemyHelper());
    }

    IEnumerator MoveEnemyHelper()
    {
        while (true)
        {
            Vector3 target = pathPoints[pointIndex].transform.position;
            
            target.y = transform.position.y;

            //Debug.Log("Target point: " + target);
            yield return StartCoroutine(WaitToGetToPoint(target));

            // increment pointIndex
            pointIndex = (pointIndex + 1) % pathPoints.Length;
        }
    }

    

    IEnumerator WaitToGetToPoint(Vector3 point)
    {
        while (transform.position.x != point.x || transform.position.z != point.z)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(point - transform.position), turnSpeed);
            point.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, point, enemySpeed * Time.deltaTime);
            yield return null;
        }
        // Debug.Log("Reached point: " + point);
    }

    void _OnPlayerSpotted(PlayerSpottedEvent e)
    {
        if (e.enemy != this.gameObject)
            return;
        StopAllCoroutines();
        // Debug.Log("Player Spotted!: " + e.player.transform.position);
        StartCoroutine(FollowPlayer(e.player));
        enemySpeed *= 2;
        
    }

    IEnumerator FollowPlayer(GameObject player)
    {
        while (!caughtPlayer)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), turnSpeed);
            Vector3 point = player.transform.position;
            point.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, point, enemySpeed * Time.deltaTime);
            yield return null;
        }
        //Debug.Log("Reached player: ");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("why is this running");
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
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
        // Debug.Log("start death");
        yield return new WaitForSeconds(1f);
        // Debug.Log("end death");
        EventBus.Publish<LevelFailEvent>(new LevelFailEvent());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{

    public DesiredPositionIsGameobject desiredPositionIsGameobject;
    public FieldOfView fieldOfView;

    public GameObject[] pathPoints;
    int pointIndex = 0;
    bool caughtPlayer = false;

    public float chaseSpeed = 6f;
    float normalSpeed;
    bool isChasingPlayer = false;


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

        while (transform.position.x != target.transform.position.x || transform.position.z != target.transform.position.z)
        {
            yield return null;
        }

        desiredPositionIsGameobject.agent.ResetPath();

        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            // Player is caught by enemy. Stop all movement and have player "die."
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

            //StartCoroutine(desiredPositionIsGameobject.PathfindingLoop());
            desiredPositionIsGameobject.agent.speed = chaseSpeed;
        }
        // change target to move to the waypoints
        else
        {
            desiredPositionIsGameobject.agent.ResetPath();

            desiredPositionIsGameobject.target = null;
            isChasingPlayer = false;
            desiredPositionIsGameobject.agent.speed = normalSpeed;
            if (pathPoints.Length < 1)
                return;

            StartCoroutine(MoveEnemy());
        }
    }

    void _OnHitObject(HitObjectEvent e)
    {
        bool packageInRange = fieldOfView.PackageInFieldOfView(e.position);
        Debug.Log("packageinrange? " + packageInRange);

        if (packageInRange && !isChasingPlayer)
        {
            StartCoroutine(DistractEnemy(e.position));
        }
    }


    IEnumerator DistractEnemy(Vector3 pos)
    {
        Debug.Log("Chase Box at: " + pos);
        desiredPositionIsGameobject.target = null;

        desiredPositionIsGameobject.agent.ResetPath();


        yield return new WaitForSeconds(3f);
        EventBus.Publish<PlayerSpottedEvent>(new PlayerSpottedEvent(null, transform.gameObject));
    }


    public bool almostEqual(float a, float b, float eps)
    {
        return Mathf.Abs(a - b) < eps;
    }
}

//StartCoroutine(desiredPositionIsGameobject.PathfindingLoop());

//while (!almostEqual(transform.position.x, hitObject.transform.position.x, 2.0f) || !almostEqual(transform.position.z, hitObject.transform.position.z, 2.0f))
//{
//    //Debug.Log("f1 " + hitObject.transform.position.x + almostEqual(transform.position.x, hitObject.transform.position.x, 2.0f));
//    //Debug.Log("f2 " + hitObject.transform.position.z + almostEqual(transform.position.z, hitObject.transform.position.z, 2.0f));
//    Debug.Log("help");
//    yield return null;
//}

//StartCoroutine(desiredPositionIsGameobject.PathfindingLoop());

//while (!almostEqual(transform.position.x, hitObject.transform.position.x, 2.0f) || !almostEqual(transform.position.z, hitObject.transform.position.z, 2.0f))
//{
//    //Debug.Log("f1 " + hitObject.transform.position.x + almostEqual(transform.position.x, hitObject.transform.position.x, 2.0f));
//    //Debug.Log("f2 " + hitObject.transform.position.z + almostEqual(transform.position.z, hitObject.transform.position.z, 2.0f));
//    Debug.Log("help");
//    yield return null;
//}

//Debug.Log("Chase Box at: " + pos);
//desiredPositionIsGameobject.target = null;
//desiredPositionIsGameobject.agent.ResetPath();

//Vector3 newRotateVal = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
//newRotateVal.y -= 90;
////look left
//while (transform.rotation.y != newRotateVal.y)
//{
//    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newRotateVal), 0.1f);
//    yield return null;
//}
////look right
//newRotateVal.y += 180;
//while (transform.rotation.y != newRotateVal.y)
//{
//    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newRotateVal), 0.1f);
//    yield return null;
//}

//EventBus.Publish<PlayerSpottedEvent>(new PlayerSpottedEvent(null, transform.gameObject));
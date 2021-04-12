using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyMovementController : MonoBehaviour
{
    #region Public Properties

    public FieldOfView fieldOfView;
    public GameObject[] pathPoints;

    public float chaseSpeed = 6f;
    [FormerlySerializedAs("isStatic")] public bool isIdle;

    public float distanceEpsilon = 0.5f;
    public float dazeTime = 10f;
    public float distractTime = 3f;

    float angleTurn = 140f;

    public int enemyID;

    #endregion

    #region Private Members

    private int _pointIndex = 0;

    private GameObject _currentTarget;
    private NavMeshAgent _agent;
    private bool _startHasRun = false;
    private float _normalSpeed;
    private float _elapsedTime = 0f;

    private bool _isChasingPlayer = false;
    private bool _isDistracted = false;
    private bool _isDazed = false;

    private bool _atBox = false;
    private bool _drawingGun = false;

    #endregion


    public TextMeshPro emoteText;

    private Subscription<PlayerSpottedEvent> _playerSpottedSubscription;

    private Subscription<HitObjectEvent> _hitObjectSubscription;


    // Start is called before the first frame update
    void Start()
    {
        PublishAnim();
        _playerSpottedSubscription = EventBus.Subscribe<PlayerSpottedEvent>(OnPlayerSpotted);
        _hitObjectSubscription = EventBus.Subscribe<HitObjectEvent>(OnHitObject);

        //desiredPositionIsGameobject = GetComponent<DesiredPositionIsGameobject>();
        fieldOfView = GetComponent<FieldOfView>();
        _agent = GetComponent<NavMeshAgent>();

        //normalSpeed = desiredPositionIsGameobject.agent.speed;
        _normalSpeed = _agent.speed;

        // set initial enemy spot to initial point and increment index
        transform.position = pathPoints[_pointIndex].transform.position;
        _pointIndex = (_pointIndex + 1) % pathPoints.Length;

        _startHasRun = true;
        PublishAnim();
    }

    private void Update()
    {
        if (isIdle)
        {
            _agent.ResetPath();
            return;
        }
        // Stop all movement if dazed.
        if (_isDazed || _isDistracted)
        {
            UpdateStatus();
            // Only reset path if we are dazed. 
            if (_isDazed || _atBox)
            {
                _agent.ResetPath();
                return;
            }
        }

        UpdateCurrentTarget();

        if (_currentTarget == pathPoints[_pointIndex])
        {
            // if a dude is turning at a pathpoint decrease fov
            if (Vector3.Angle(transform.forward, transform.position - _currentTarget.transform.position) < angleTurn)
            {
                fieldOfView.cur_field_radius = fieldOfView.field_radius/2;
            }
            else
            {
                fieldOfView.cur_field_radius = fieldOfView.field_radius;
            }
        }

        // Move the enemy to the target destination. 
        if (_currentTarget != null)
        {
            _agent.SetDestination(_currentTarget.transform.position);
        }
    }

    private void UpdateCurrentTarget()
    {
        //Debug.Log("Update Target!");
        if (_currentTarget == null)
        {
            _currentTarget = pathPoints[_pointIndex];
        } else if (_currentTarget == pathPoints[_pointIndex] && IsAtPathPoint())
        {
            _pointIndex = (_pointIndex + 1) % pathPoints.Length;
            _currentTarget = pathPoints[_pointIndex];
        }
    }

    private void UpdateStatus()
    {
        if (_isDazed || (_isDistracted && IsAtPathPoint()))
        {
            // update our current elapsed time
            _elapsedTime += Time.deltaTime;
        }

        if (_isDistracted && IsAtPathPoint())
        {
            //Debug.Log("At Box!");
            _atBox = true;
            PublishAnim();
        }

        // Check if any enemy statuses are "expired"
        if (_isDazed && _elapsedTime >= dazeTime)
        {
            _isDazed = false;
            emoteText.text = "";
            _currentTarget = null;
            _elapsedTime = 0f;
            PublishAnim();
        }
        else if (_isDistracted && _elapsedTime >= distractTime)
        {
            _isDistracted = false;
            emoteText.text = "";
            _currentTarget = null;
            _elapsedTime = 0f;
            _atBox = false;
            PublishAnim();
        }
        
    }

    private bool IsAtPathPoint()
    {
        // Check if we are close enough to the path point to consider us "at the point"
        return !_agent.pathPending && _agent.remainingDistance < distanceEpsilon;
    }

    private void PublishAnim()
    {
        //Animation publisher
        //Debug.Log("idle: " + isIdle + " atbox: " + _atBox + " dazed: " + _isDazed + " drawGun: " + _drawingGun);
        EventBus.Publish(new EnemyStateEvent(!isIdle, _atBox, _isDazed, _drawingGun, enemyID));
    }

    private void OnPlayerSpotted(PlayerSpottedEvent e)
    {
        // If we see the player, change the currentTarget to player.
        if (gameObject != e.enemy) return;
        if (_isDazed) return;

        if (e.player != null)
        {
            _isChasingPlayer = true;
            _agent.speed = chaseSpeed;
            fieldOfView.cur_view_range = 360f;
            emoteText.text = "!";
        }
        else
        {
            _isChasingPlayer = false;
            _agent.speed = _normalSpeed;
            fieldOfView.cur_view_range = fieldOfView.view_range;
            emoteText.text = "";
        }

        _currentTarget = e.player;
    }

    private void OnHitObject(HitObjectEvent e)
    {
        if (_isDazed) return;

        PublishAnim();
        bool packageInRange = fieldOfView.PackageInFieldOfView(e.sourceObject.transform.position);
        //Debug.Log("package In Range? " + packageInRange);
        //Debug.Log("IsDistracted: " + _isDistracted);
        
        // Check if package hit the enemy
        if (e.hitObject == gameObject)
        {
            // Daze enemy hit by package
            _isDazed = true;
            _isChasingPlayer = false;
            _isDistracted = false;
            emoteText.text = "*";
            PublishAnim();
            //Debug.Log("Hit Enemy!");
        }
        else if (packageInRange)
        {
            // Package is in field of view.
            // If we are already distracted, ignore 
            if (_isDistracted || _isChasingPlayer) return;

            // Become "distracted"
            _isDistracted = true;
            _isChasingPlayer = false;
            _isDazed = false;
            emoteText.text = "?";
            PublishAnim();

            // Set current target to be the object.
            _currentTarget = e.sourceObject;
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !_isDazed)
        {
            var playerMove = other.gameObject.GetComponent<PlayerMove>();
            if (playerMove == null) return;
            
            playerMove.baseMovementSpeed = 0f;
            playerMove.rotationSpeed = 0f;
            playerMove.enabled = false;
            EventBus.Publish(new MovementEvent(false, false, true));
            isIdle = true;
            StartCoroutine(WaitToDie());
        }
        
    }
    
    IEnumerator WaitToDie()
    {
        _drawingGun = true;
        PublishAnim();
        yield return new WaitForSeconds(1f);
        EventBus.Publish<LevelFailEvent>(new LevelFailEvent(false));
    }
    //
    // IEnumerator MoveEnemy()
    // {
    //     while (true)
    //     {
    //         
    //
    //         GameObject target = pathPoints[pointIndex];
    //
    //         // wait until enemy has reached the target
    //         yield return StartCoroutine(WaitToGetToPoint(target, 2f));
    //
    //         // increment pointIndex
    //         pointIndex = (pointIndex + 1) % pathPoints.Length;
    //     }
    // }
    //
    // IEnumerator WaitToGetToPoint(GameObject target, float eps)
    // {
    //     // set desired target
    //     desiredPositionIsGameobject.target = target;
    //
    //     if (target != null) {
    //         while (!almostEqual(transform.position.x, target.transform.position.x, eps) || !almostEqual(transform.position.z, target.transform.position.z, eps))
    //         {
    //             yield return null;
    //         }
    //
    //         desiredPositionIsGameobject.agent.ResetPath();
    //     }
    //
    //
    //     
    //
    //     yield return null;
    // }
    //
    // // Collisions
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.collider.gameObject.CompareTag("Player") && !isDazed)
    //     {
    //         // Player is caught by enemy. Stop all movement and have player "die."
    //         StopAllCoroutines();
    //         desiredPositionIsGameobject.StopAllCoroutines();
    //         PlayerMove playerMove = collision.collider.gameObject.GetComponent<PlayerMove>();
    //         if (playerMove != null)
    //         {
    //             playerMove.baseMovementSpeed = 0f;
    //             playerMove.rotationSpeed = 0f;
    //             playerMove.enabled = false;
    //         }
    //         StartCoroutine(WaitToDie());
    //     }
    // }
    //
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player") && !isDazed)
    //     {
    //         // Player is caught by enemy. Stop all movement and have player "die."
    //         StopAllCoroutines();
    //         desiredPositionIsGameobject.StopAllCoroutines();
    //         PlayerMove playerMove = other.gameObject.GetComponent<PlayerMove>();
    //         if (playerMove != null)
    //         {
    //             playerMove.baseMovementSpeed = 0f;
    //             playerMove.rotationSpeed = 0f;
    //             playerMove.enabled = false;
    //         }
    //         StartCoroutine(WaitToDie());
    //     }
    // }
    //
    // IEnumerator WaitToDie()
    // {
    //     drawingGun = true;
    //     PublishAnim();
    //     yield return new WaitForSeconds(1f);
    //     bool wasFall = false;
    //     EventBus.Publish<LevelFailEvent>(new LevelFailEvent(wasFall));
    // }
    //
    // // Events
    //
    // void _OnPlayerSpotted(PlayerSpottedEvent e)
    // {
    //     // check to see if it is the right enemy
    //     if (e.enemy != this.gameObject || isDazed)
    //         return;
    //
    //     // change target to be the player
    //     if (e.player)
    //     {
    //         StopAllCoroutines();
    //         Debug.Log("Player Spotted!: " + e.player.transform.position);
    //         desiredPositionIsGameobject.agent.ResetPath();
    //
    //         desiredPositionIsGameobject.target = e.player;
    //         isChasingPlayer = true;
    //         
    //         emoteText.text = "!";
    //
    //         desiredPositionIsGameobject.agent.speed = chaseSpeed;
    //     }
    //     // change target to move to the waypoints
    //     else
    //     {
    //         desiredPositionIsGameobject.agent.ResetPath();
    //
    //         desiredPositionIsGameobject.target = null;
    //         isChasingPlayer = false;
    //         
    //         emoteText.text = "";
    //
    //         desiredPositionIsGameobject.agent.speed = normalSpeed;
    //         if (pathPoints.Length < 1)
    //             return;
    //
    //         StartCoroutine(MoveEnemy());
    //     }
    // }
    //
    // void _OnHitObject(HitObjectEvent e)
    // {
    //     PublishAnim();
    //     Debug.Log(e.hitObject); // ground
    //     Debug.Log(e.sourceObject); // golden package
    //
    //     bool packageInRange = fieldOfView.PackageInFieldOfView(e.sourceObject.transform.position);
    //     Debug.Log("packageinrange? " + packageInRange);
    //
    //     if (packageInRange && !isDazed)
    //     {
    //         Debug.Log("check");
    //         if (e.hitObject.CompareTag("Enemy"))
    //         {
    //             // make sure it is this enemy
    //             if (e.hitObject != this.gameObject)
    //                 return;
    //
    //             Debug.Log("Daze Enemy");
    //             StopAllCoroutines();
    //
    //             StartCoroutine(DazeEnemy());
    //         }
    //         else if(!isStatic && !isDistracted && !isChasingPlayer)
    //         {
    //             Debug.Log("Distract Enemy");
    //             StopAllCoroutines();
    //
    //             StartCoroutine(DistractEnemy(e.sourceObject));
    //         }
    //     }
    //
    // }
    //
    // IEnumerator DistractEnemy(GameObject box)
    // {
    //     isDistracted = true;
    //     emoteText.text = "?";
    //     PublishAnim();
    //
    //     Debug.Log("Chase Box at: " + box.transform.position);
    //     desiredPositionIsGameobject.target = box;
    //
    //     desiredPositionIsGameobject.agent.ResetPath();
    //
    //     yield return StartCoroutine(WaitToGetToPoint(box, 2.5f));
    //     atBox = true;
    //     PublishAnim();
    //     
    //     desiredPositionIsGameobject.agent.speed = 0f;
    //     yield return new WaitForSeconds(distractTime);
    //     atBox = false;
    //     PublishAnim();
    //     
    //     desiredPositionIsGameobject.agent.speed = normalSpeed;
    //     isDistracted = false;
    //     PublishAnim();
    //
    //     EventBus.Publish<PlayerSpottedEvent>(new PlayerSpottedEvent(null, transform.gameObject));
    //     emoteText.text = "";
    // }
    //
    // IEnumerator DazeEnemy()
    // {
    //     isDazed = true;
    //     isChasingPlayer = false;
    //     isDistracted = false;
    //     PublishAnim();
    //     emoteText.text = "*";
    //     desiredPositionIsGameobject.target = null;
    //
    //     desiredPositionIsGameobject.agent.ResetPath();
    //
    //     yield return new WaitForSeconds(dazeTime);
    //
    //     isDazed = false;
    //     EventBus.Publish<PlayerSpottedEvent>(new PlayerSpottedEvent(null, transform.gameObject));
    //     PublishAnim();
    //     emoteText.text = "";
    // }
    //
    //
    // public bool almostEqual(float a, float b, float eps)
    // {
    //     return Mathf.Abs(a - b) < eps;
    // }

    private void OnDrawGizmos()
    {
        if (!_startHasRun) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < _agent.path.corners.Length - 1; i++)
        {
            Gizmos.DrawLine(_agent.path.corners[i], _agent.path.corners[i + 1]);
            Gizmos.DrawSphere(_agent.path.corners[i], .25f);
        }
    }
}
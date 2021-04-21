using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using System.Linq;

public class ThrowPeriodically : MonoBehaviour
{

    Subscription<LevelClearEvent> levelClearSubscription;

    private GameObject player;

    public LineRenderer lineRenderer;

    public float throwAngle = 45f;


    //holobox stuff
    Vector3 holoboxPos;
    Vector3 halfExtents = new Vector3(0.5f, 0.5f, 0.5f);
    public GameObject holoBox;

    public GameObject projectilePrefab;
    private GameObject projectileInstance;


    private float aimDuration = 6f;
    private float time = 0;

    private GameObject idle;
    private GameObject throwing;
    private GameObject turning;
    private GameObject dying;

    private bool dead = false;

    private bool starting = true;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        idle = transform.Find("Idle").gameObject;
        throwing = transform.Find("Throwing").gameObject;
        turning = transform.Find("Turning").gameObject;
        dying = transform.Find("Boss Dying").gameObject;

        levelClearSubscription = EventBus.Subscribe<LevelClearEvent>(OnDeath);

        holoBox.transform.parent = null;

        if (Globals.debug) {
            lineRenderer.enabled = true;
        }
    }

    void OnDeath(LevelClearEvent _event) {
        throwing.SetActive(false);
        idle.SetActive(false);
        holoBox.SetActive(false);
        turning.SetActive(false);

        GetComponent<FacePlayer>().enabled = false;

        dying.SetActive(true);
        dead = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (dead) return;

        if (!projectilePrefab) return;

        // Spawn
        if (!projectileInstance) {
            SpawnItem();
        }

        time += Time.deltaTime;

        // Aim
        DrawTrajectoryPath(time > aimDuration);

        if (time > aimDuration) {
            time = 0f;
            ThrowItem();
            starting = false;
        }
    }

    public void OnDamage() {
        aimDuration -= 0.5f;
    }


    void ThrowItem()
    {
        // Throw projectile
        Vector3 force = CalculateVelocity();
        GameObject item = projectileInstance;
        MeshRenderer[] mr = item.GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in mr)
        {
            mesh.enabled = true;
        }
        Projector[] pr = item.GetComponentsInChildren<Projector>();
        foreach (var proj in pr)
        {
            proj.enabled = true;
        }
        // Re-enable rigidbody and collider.
        Rigidbody rb = item.GetComponent<Rigidbody>();

        Collider col = item.GetComponent<Collider>();

        // Set the parent back to null
        item.transform.parent = null;
        rb.isKinematic = false;
        col.enabled = true;

        // Add Component to the objectz.
        //item.AddComponent<OnCollisionEvent>();
        OnCollisionEvent collisionEvent = item.AddComponent<OnCollisionEvent>();

        // Disable LineRenderer
        throwing.SetActive(false);
        idle.SetActive(true);
        // holoBox.SetActive(false);
        turning.SetActive(false);
        projectileInstance.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        projectileInstance = null;

    }


    private void SpawnItem()
    {
        if (projectileInstance) return;
        GameObject item = projectileInstance = Instantiate(projectilePrefab, transform);
        if (item.GetComponent<Respawnable>()) {
            item.GetComponent<Respawnable>().enabled = false;
        }

        item.transform.localPosition = new Vector3(2, 2, 0); 

        //Disable the rigidbody and rest velocities 
        Rigidbody rb = item.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Disable collider of grabbed object.
        Collider col = item.GetComponent<Collider>();
        col.enabled = false;
        MeshRenderer[] mr = item.GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in mr)
        {
            mesh.enabled = false;
        }
        Projector[] pr = item.GetComponentsInChildren<Projector>();
        foreach (var proj in pr)
        {
            proj.enabled = false;
        }
        //lineRenderer.enabled = false;

    }


    private Vector3 CalculateVelocity(bool mass = false)
    {
        float angle = throwAngle * Mathf.Deg2Rad;
        float tan = Mathf.Tan(angle);
        float sin = Mathf.Sin(angle);

        // Acceleration
        float a_y = Physics.gravity.y;
        
        // Vertical distance
        Vector3 displacement = player.transform.position;
        if (projectileInstance) {
            displacement -= projectileInstance.transform.position;
        }
        else {
            displacement -= transform.position;
        }
        float dy = displacement.y;
        displacement.y = 0;

        // Lateral Distance
        float dx = displacement.magnitude;


        float v_i_sqr = Mathf.Abs((a_y * dx * dx * tan) / (dy - 2f * dx * sin * sin));
        float v_i = Mathf.Sqrt(v_i_sqr);

        Vector3 angleDir = new Vector3(0, sin, 0);
        Vector3 impulse = v_i * (displacement.normalized + angleDir).normalized;

        // Multiply by mass if applicable
        if (mass && projectileInstance) {
            impulse *= projectileInstance.GetComponent<Rigidbody>().mass;
        }

        return impulse;
    }


    private void DrawTrajectoryPath(bool reset_holobox = false)
    {
        if (!starting)
            throwing.SetActive(true);
        else
        {
            turning.SetActive(true);
            
        }
        idle.SetActive(false);
        // ---- Draw trajectory path -----
        // To draw the trajectory path we need to simulate the projectile position across set intervals.
        // First, we need to calculate the total time the projectile will take before landing
        // This is given by the equation t = (vsin(θ) + sqr((vsin(θ)^2 + 2gy_0)) / g where
        // θ = Angle of the projectile
        // y_0 = the initial height of the projectile
        // v = the magnitude of the velocity vector
        // g = gravity

        if (! (Globals.debug || reset_holobox) ) return;

        List<Vector3> path = new List<Vector3>();
        Vector3 position = projectileInstance.transform.position;
        Vector3 velocity = CalculateVelocity(true);
        //Debug.Log(velocity);
        float v = velocity.magnitude;
        // Calculate magnitude
        // Since Physics.gravity.y returns a negative value, we have to convert to absolute value. 
        float totalTime = (v * Mathf.Sin(throwAngle) +
                           Mathf.Sqrt(Mathf.Pow(v * Mathf.Sin(throwAngle), 2) +
                                      Mathf.Abs(2 * Physics.gravity.y * position.y)));
        totalTime /= Mathf.Abs(Physics.gravity.y);
        //Debug.Log(totalTime);

        // Next, we need to simulate the flight path by calculating the position and 
        // velocity vectors over set time intervals.
        // For each interval, we add the position to the path list so we can draw the trajectory.
        float timeStep = Time.fixedDeltaTime;
        for (float t = 0f; t < totalTime; t += timeStep)
        {
            velocity += Physics.gravity * timeStep;
            position += velocity * timeStep;
            path.Add(position);
        }

        // Now, draw the trajectory using a LineRenderer.
        lineRenderer.positionCount = path.Count;
        lineRenderer.SetPositions(path.ToArray());

        if (!reset_holobox) return;

        holoBox.SetActive(true);
        foreach (var item in path)
        {
            
            if (HoloBoxOverLap(item))
            {
                holoboxPos = item;
               
                break;
            }
        }
        holoBox.transform.position = holoboxPos;
                
    }

    private bool HoloBoxOverLap(Vector3 point)
    {
        LayerMask mask = LayerMask.GetMask("Grabbable Object") + LayerMask.GetMask("Player") + LayerMask.GetMask("Enemy") + LayerMask.GetMask("ObjectPickedUp");
        if (Physics.OverlapBox(point, halfExtents, transform.rotation, ~mask).Length > 0)
        
        {
           // foreach (var item in Physics.OverlapBox(point, halfExtents, transform.rotation, ~mask))
            //{
              //  Debug.Log(item.name);
            //}
            return true;
        }
        return false;
    }


}

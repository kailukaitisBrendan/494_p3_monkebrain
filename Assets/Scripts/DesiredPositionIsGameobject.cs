using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DesiredPositionIsGameobject : MonoBehaviour
{

    public GameObject target;
    public NavMeshAgent agent;
    public float pathfinding_refresh_interval_sec = 1.0f;

    bool startHasRun = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startHasRun = true;
        // StartCoroutine(PathfindingLoop());
    }

    public IEnumerator PathfindingLoop()
    {
        while (true)
        {
            GameObject target_go = target;
            if (target_go != null)
            {
                Vector3 destination = target_go.transform.position;
                destination.y = transform.position.y;
                agent.SetDestination(destination);
            }
            //yield return new WaitForSeconds(pathfinding_refresh_interval_sec);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!startHasRun) return;

        Gizmos.color = Color.red;
        for(int i = 0; i < agent.path.corners.Length - 1; i++)
        {
            Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
            Gizmos.DrawSphere(agent.path.corners[i], .25f);
        }

    }
}

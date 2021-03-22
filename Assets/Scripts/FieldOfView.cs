﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    public GameObject visibleTargetPlayer;
    public bool playerSpotted = false;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public LayerMask obstacleMaskForBox;

    public float field_radius = 5f;
    public float view_range = 75;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool spottedValueChanged = playerSpotted;
        FindVisibleTargets();

        if (spottedValueChanged == playerSpotted)
            return;

        if(playerSpotted)
        {
            EventBus.Publish<PlayerSpottedEvent>(new PlayerSpottedEvent(visibleTargetPlayer, transform.gameObject));
        }
        else
        {
            EventBus.Publish<PlayerSpottedEvent>(new PlayerSpottedEvent(null, transform.gameObject));
        }
    }

    void FindVisibleTargets()
    {
        visibleTargetPlayer = null;
        playerSpotted = false;

        Collider[] targets = Physics.OverlapSphere(transform.position, field_radius, targetMask);
        for (int i = 0; i < targets.Length; ++i)
        {
            Vector3 objectToTargetDir = (targets[i].transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, objectToTargetDir) < view_range / 2)
            {
                //make sure there are no obstacles/walls in the way
                if (!Physics.Raycast(transform.position, objectToTargetDir, Vector3.Distance(transform.position, targets[i].transform.position), obstacleMask))
                {
                    visibleTargetPlayer = targets[i].gameObject;
                    playerSpotted = true;
                }
            }
        }
    }

    public bool PackageInFieldOfView(Vector3 package_position)
    {

        bool inRange = false;
        float distance_to_package = Vector3.Distance(transform.position, package_position);
        Vector3 package_to_enemy_dir = (package_position - transform.position).normalized;
        //make sure there are no obstacles/walls in the way and package is within radius
        if (distance_to_package <= field_radius && !Physics.Raycast(transform.position, package_to_enemy_dir, distance_to_package, obstacleMaskForBox))
        {
            inRange = true;
        }

        return inRange;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, field_radius);
    }
}

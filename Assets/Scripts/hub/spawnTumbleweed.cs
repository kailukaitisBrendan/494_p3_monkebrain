using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnTumbleweed : MonoBehaviour
{
    float time_spawn;
    float time_since;
    public GameObject tumbleweed;
    GameObject inst;
    void Start() {
        time_spawn = Time.time;
        time_since = Time.time + 0.5f;
    }
    void FixedUpdate()
    {
        // at random intervals, spawn a new tumbleweed
        if (Time.time - time_spawn > time_since) {
            time_since = Time.time + Random.Range(1.0f,2.0f);
            inst = Instantiate(tumbleweed, new Vector3 (transform.position.x - 30f,0.5f,transform.position.z + 30f), Quaternion.identity);
            inst.GetComponent<tumbleweedMovement>().SetCam(gameObject);
            inst = Instantiate(tumbleweed, new Vector3 (transform.position.x - 30f,0.5f,transform.position.z - 30f), Quaternion.identity);
            inst.GetComponent<tumbleweedMovement>().SetCam(gameObject);
        }
    }
}

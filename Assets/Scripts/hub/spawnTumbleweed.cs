using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnTumbleweed : MonoBehaviour
{
    float time_spawn;
    float time_since;
    public GameObject tumbleweed;
    public float y_coord = 0.5f;
    public float x_radius = 30f;
    public float z_radius = 30f;
    GameObject inst;
    void Start() {
        Time.timeScale = 1.0f;
        time_spawn = Time.time;
        time_since = 0.2f;
    }
    void FixedUpdate() {
        // at random intervals, spawn a new tumbleweed
        if (Time.time - time_spawn > time_since) {
            time_since = Random.Range(0.8f,3.0f);
            time_spawn = Time.time;
            inst = Instantiate(tumbleweed, new Vector3 (transform.position.x - x_radius,y_coord,transform.position.z + z_radius), Quaternion.identity);
            inst.GetComponent<tumbleweedMovement>().SetCam(gameObject);
            inst = Instantiate(tumbleweed, new Vector3 (transform.position.x - x_radius,y_coord,transform.position.z - z_radius), Quaternion.identity);
            inst.GetComponent<tumbleweedMovement>().SetCam(gameObject);
        }
    }
}

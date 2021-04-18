using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tumbleweedMovement : MonoBehaviour
{
    public ParticleSystem poof;
    public GameObject skin;
    public GameObject particlesystem1;
    GameObject cam;
    float time_spawn;
    float time_run = 6.0f;
    float speed = 8f;
    float rot;
    public bool dead;
    void Start() {
        // generate random movement vector
        rot = Random.Range(-180f,180f);
        transform.rotation = Quaternion.Euler(0, rot, 0);
        time_spawn = Time.time;
        dead = false;
    }

    // Update is called once per frame
    void FixedUpdate() {
        // move in random direction
        if (!dead) {
            transform.position += new Vector3(speed * Time.deltaTime * Mathf.Sin(rot),0f,speed * Time.deltaTime* Mathf.Cos(rot));
        }
        if (cam != null) {
            if (Mathf.Abs(gameObject.transform.position.x - cam.transform.position.x) > 200f 
            || Mathf.Abs(gameObject.transform.position.z - cam.transform.position.z) > 200f) {
                dead = true;
                Destroy(gameObject);
            }
        }
        if (Time.time - time_spawn > time_run && !poof.IsAlive()) {

            if (dead) {
                Destroy(gameObject);
            }

            dead = true;
            CreateDust();
            skin.SetActive(false);
            particlesystem1.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.layer != 12 && !poof.IsAlive()) {
            dead = true;
            CreateDust();
            skin.SetActive(false);
            particlesystem1.SetActive(false);
        }
    }

    public void SetCam(GameObject came) {
        cam = came;
    }

    void CreateDust() {
        poof.Play();
    }
    void StopDust() {
        poof.Stop();
    }
}

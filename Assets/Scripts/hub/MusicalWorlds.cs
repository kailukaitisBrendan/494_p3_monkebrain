using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalWorlds : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public GameObject c;
    public GameObject wagon;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        a.SetActive(true);
        b.SetActive(false);
        c.SetActive(false);
    }
    void Update()
    {
        if (wagon.transform.position.x >= 75f) {
            a.SetActive(true);
            c.SetActive(false);
            // if b is on turn down b and turn down a
            if (b.activeSelf) {
                //if (a.GetComponent<AudioSource>().volume > 0.79f)
                //    a.GetComponent<AudioSource>().volume = 0.1f;
                b.GetComponent<AudioSource>().volume -= Time.deltaTime * 0.5f;
            }
            // if b too low disable b
            if (b.GetComponent<AudioSource>().volume < 0.1f) {
                b.SetActive(false);
            }
            // turn up a
            if (a.GetComponent<AudioSource>().volume < 0.8f)
                a.GetComponent<AudioSource>().volume += Time.deltaTime * 0.5f;
        }
        if (wagon.transform.position.x < 75f && wagon.transform.position.x > -11f) {
            b.SetActive(true);
            if (a.activeSelf) {
                //if (b.GetComponent<AudioSource>().volume > 0.79f)
                //    b.GetComponent<AudioSource>().volume = 0.1f;
                a.GetComponent<AudioSource>().volume -= Time.deltaTime * 0.5f;
            }
            if (a.GetComponent<AudioSource>().volume < 0.1f) {
                a.SetActive(false);
            }
            if (c.activeSelf) {
                //if (b.GetComponent<AudioSource>().volume > 0.79f)
                //    b.GetComponent<AudioSource>().volume = 0.1f;
                c.GetComponent<AudioSource>().volume -= Time.deltaTime * 0.5f;
            }
            if (c.GetComponent<AudioSource>().volume < 0.1f) {
                c.SetActive(false);
            }
            if (b.GetComponent<AudioSource>().volume < 0.8f)
                b.GetComponent<AudioSource>().volume += Time.deltaTime * 0.5f;
        }
        if (wagon.transform.position.x <= -11f) {
            a.SetActive(false);
            c.SetActive(true);
            if (b.activeSelf) {
                //if (c.GetComponent<AudioSource>().volume > 0.79f)
                //    c.GetComponent<AudioSource>().volume = 0.1f;
                b.GetComponent<AudioSource>().volume -= Time.deltaTime * 0.5f;
            }
            if (b.GetComponent<AudioSource>().volume < 0.1f) {
                b.SetActive(false);
            }
            c.SetActive(true);
            if (c.GetComponent<AudioSource>().volume < 0.8f)
                    c.GetComponent<AudioSource>().volume += Time.deltaTime * 0.5f;
        }
    }
}

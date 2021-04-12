using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalWorlds : MonoBehaviour
{
    public GameObject a;
    public GameObject b;
    public GameObject c;
    public GameObject d;
    public GameObject wagon;
    public static MusicalWorlds instance = null;  
    void Start()
    {
        //Check if instance already exists
        if (instance == null) {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this) {
            instance.gameObject.SetActive(true);
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        a.SetActive(true);
        b.SetActive(false);
        c.SetActive(false);
    }
    void Update()
    {
        if (wagon.transform.position.x >= 75f) {
            d.SetActive(false);
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
        if (wagon.transform.position.x < 75f && wagon.transform.position.x > 0f) {
            d.SetActive(false);
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
        if (wagon.transform.position.x < 0f && wagon.transform.position.x > -40f) {
            a.SetActive(false);
            c.SetActive(true);
            if (d.activeSelf) {
                //if (b.GetComponent<AudioSource>().volume > 0.79f)
                //    b.GetComponent<AudioSource>().volume = 0.1f;
                d.GetComponent<AudioSource>().volume -= Time.deltaTime * 0.5f;
            }
            if (d.GetComponent<AudioSource>().volume < 0.1f) {
                d.SetActive(false);
            }
            if (b.activeSelf) {
                //if (b.GetComponent<AudioSource>().volume > 0.79f)
                //    b.GetComponent<AudioSource>().volume = 0.1f;
                b.GetComponent<AudioSource>().volume -= Time.deltaTime * 0.5f;
            }
            if (b.GetComponent<AudioSource>().volume < 0.1f) {
                b.SetActive(false);
            }
            if (c.GetComponent<AudioSource>().volume < 0.8f)
                c.GetComponent<AudioSource>().volume += Time.deltaTime * 0.5f;
        }
        if (wagon.transform.position.x <= -40f) {
            a.SetActive(false);
            d.SetActive(true);
            b.SetActive(false);
            if (c.activeSelf) {
                //if (c.GetComponent<AudioSource>().volume > 0.79f)
                //    c.GetComponent<AudioSource>().volume = 0.1f;
                c.GetComponent<AudioSource>().volume -= Time.deltaTime * 0.5f;
            }
            if (c.GetComponent<AudioSource>().volume < 0.1f) {
                c.SetActive(false);
            }
            d.SetActive(true);
            if (d.GetComponent<AudioSource>().volume < 0.8f)
                    d.GetComponent<AudioSource>().volume += Time.deltaTime * 0.5f;
        }
    }
}

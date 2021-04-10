using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSound : MonoBehaviour
{
    AudioSource a;
    public static MainMenuSound instance = null; 
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<AudioSource>();
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
        GameObject g = GameObject.Find("SoundController");
        if (g != null) {
            instance.gameObject.SetActive(true);
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main_Menu") {
            return;
        }
        if (SceneManager.GetActiveScene().name == "Intro" && a.volume > 0.07f) {
            a.volume -= Time.deltaTime * 0.08f;
        }
        if (SceneManager.GetActiveScene().name == "Hub") {
            Destroy(this.gameObject);
        }
    }
}

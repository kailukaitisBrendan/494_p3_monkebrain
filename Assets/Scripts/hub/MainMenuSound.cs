using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSound : MonoBehaviour
{
    AudioSource a;
    // Start is called before the first frame update
    void Start()
    {
        a = GetComponent<AudioSource>();
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

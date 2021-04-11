using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player);
    }

    // Update is called once per frame
    void Update ()
    {
        if (false){//player == null) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else {
            transform.LookAt(player.transform);
        }
    }
}

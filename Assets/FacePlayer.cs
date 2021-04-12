using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private GameObject player;
    private bool turned;
    Quaternion backward;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(player);
        backward = transform.rotation;
        backward.y = -backward.y;
    }

    // Update is called once per frame
    void Update ()
    {
        if (!turned){//player == null) {
            StartCoroutine(WaitThenLook());
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, backward, 2);
        }
        else {
            Vector3 direction = player.transform.position;
            direction.y = transform.position.y;
            transform.LookAt(direction);
        }
    }

    IEnumerator WaitThenLook()
    {
        yield return new WaitForSeconds(3f);
        turned = true;
    }

}

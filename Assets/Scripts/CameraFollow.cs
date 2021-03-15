using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    public Vector3 offset = new Vector3(0, 7, -5);
    public float radius = 4.0f;
    public float moveSpeed = 0.5f;
    public float y_axis_position;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    float t = 0.0f;
    void FixedUpdate()
    {
        
        t -= Input.GetAxis("Mouse X") * moveSpeed;

        Vector3 playerXZ = Vector3.zero;
        playerXZ.x += player.transform.position.x;
        playerXZ.z += player.transform.position.z;

        Vector3 playerY = Vector3.zero;
        playerY.y += player.transform.position.y;


        Vector3 xz_position = new Vector3(Mathf.Cos(t), 0.0f, Mathf.Sin(t)) * radius + playerXZ;
        Vector3 y_position = new Vector3(0.0f, y_axis_position, 0.0f) + playerY;

        transform.position = xz_position + y_position;


    }

    private void LateUpdate()
    {
        transform.LookAt(player.transform.position, Vector3.up);
    }
}

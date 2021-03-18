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
    public float min_y = 0;
    public float max_y = 9;
    public LayerMask mask;
    private RaycastHit camhit;
    public float lerpSpeed = 0.5f;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }



    float t = 0.0f;
    void FixedUpdate()
    {
        
        if (CheckWall())
        {
            
            
            radius = Vector3.Distance(camhit.point, player.transform.position);
            //Debug.Log("True");
        }
        else
        {
            radius = 4f;
            
        }


        t -= Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;

        Vector3 playerXZ = Vector3.zero;
        playerXZ.x += player.transform.position.x;
        playerXZ.z += player.transform.position.z;

        Vector3 playerY = Vector3.zero;
        playerY.y += player.transform.position.y;
       

        if (y_axis_position <= max_y && y_axis_position >= min_y)
        {
            
            y_axis_position += Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime * 2;
        }
        if(y_axis_position > max_y)
        {
            y_axis_position = 9;
        }
        if(y_axis_position < min_y)
        {
            y_axis_position = 0;
        }

        Vector3 xz_position = new Vector3(Mathf.Cos(t), 0.0f, Mathf.Sin(t)) * radius + playerXZ;
        Vector3 y_position = new Vector3(0.0f, y_axis_position, 0.0f) + playerY;
        Vector3 newPos = xz_position + y_position;


        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * lerpSpeed);

    }

    private void LateUpdate()
    {
        transform.LookAt(player.transform.position, Vector3.up);
    }


    private bool CheckWall()
    {

        Vector3 checker = transform.position;
        checker.y = player.transform.position.y;
        float dist = 4f;
        //Debug.DrawRay(player.transform.position, checker - player.transform.position, Color.yellow);
        
        if(Physics.Raycast(player.transform.position, checker - player.transform.position, out camhit, dist, ~mask)){
            
            return true;
        }
        //Debug.Log(false);
        return false;

    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassRaiseMovement : MonoBehaviour
{
    Vector3 start;
    Vector3 destination;
    public GameObject startPos;
    public GameObject endPos;
    public float moveSpeed = 0.56f;
    bool goingUp = true;
    // Start is called before the first frame update
    void Start()
    {
        start = startPos.transform.position;
        destination = endPos.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos;


        if (transform.position.y > (destination.y - 0.01f))  StartCoroutine(WaitFalse());
        else if (transform.position.y < (start.y + 0.01f)) StartCoroutine(WaitTrue());
        

        if (goingUp)
            newPos = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);
        else
            newPos = Vector3.MoveTowards(transform.position, start, Time.deltaTime * moveSpeed);

        transform.position = newPos;
    }

    IEnumerator WaitFalse()
    {
        yield return new WaitForSeconds(0.1f);
        goingUp = false;
    }

    IEnumerator WaitTrue()
    {
        yield return new WaitForSeconds(0.1f);
        goingUp = true;
    }
}


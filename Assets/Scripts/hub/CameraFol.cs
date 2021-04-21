using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraFol : MonoBehaviour
{
    public GameObject player;

    public Vector3 offset = new Vector3(0, 4, -5);
    public float baseRadius = 4.0f;
    public float radius = 4.0f;
    public Vector2 sensitivity;
    public float y_axis_position;
    public float min_y = 0;
    public float max_y = 4.5f;
    public LayerMask mask;
    private RaycastHit camhit;
    public float lerpSpeed = 2f;

    public static CameraFol instance = null;  
    void Awake() {
        //Check if instance already exists
        if (instance == null) {
            //if not, set instance to this
            instance = this;
            // teleport to above wagon
            transform.position = new Vector3(player.transform.position.x + 4f,player.transform.position.y + 11f,player.transform.position.z);
        }
        //If instance already exists and it's not this:
        else if (instance != this) {
            // teleport to above wagon
            if (SceneManager.GetActiveScene().name == "HubMine" 
                    && instance.player.transform.position.x < -20f
                    && instance.player.transform.position.x > -50f) {
                PlayerPrefs.SetInt("Mine",1);
                transform.position = new Vector3(player.transform.position.x + 4f,player.transform.position.y + 11f,player.transform.position.z);
                Destroy(instance.gameObject);
                instance = this;
            } else {
                instance.gameObject.transform.position = new Vector3(instance.player.transform.position.x + 4f,instance.player.transform.position.y + 11f,instance.player.transform.position.z);
                instance.gameObject.SetActive(true);
                Destroy(gameObject);
            }
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    [Range(0,1)]
    public float wallBuffer = 0.5f;

    RaycastHit hit;
    Renderer rend;
    RaycastHit[] hits = new RaycastHit[0];
    RaycastHit[] old_hits = new RaycastHit[0];
    Hashtable shaders = new Hashtable();

    float t = 0.0f;
    void Update()
    {
        // START RAYCAST TO MAKE OBJECTS IN HUB TRANSPARENT
        // raycast to check if there is something between player and camera
        // credit to the unity documentation for this section
        hits = Physics.RaycastAll(transform.position, transform.forward, 15.5f);
        for (int i = 0; i < hits.Length; i++)
        {
            hit = hits[i];
            rend = hit.transform.GetComponent<Renderer>();
            if (rend)
            {
                // Add list of shaders to shader hash table
                if (!shaders.ContainsKey(rend)) {
                    Shader[] rend_shaders = rend.materials.Select(material => material.shader).ToArray();
                    shaders.Add(rend, rend_shaders);
                }

                // Change the material of all hit colliders
                // to use a transparent shader.
                for (int j = 0; j < rend.materials.Length; j++) {
                    rend.materials[j].shader = Shader.Find("Transparent/Diffuse");
                    Color tempColor = rend.materials[j].color;
                    tempColor.a = 0.3f;
                    rend.materials[j].color = tempColor;
                } 
            }
        }
        // check if there are objects that no longer need to be transparent
        for (int i = 0; i < old_hits.Length; i++)
        {
            hit = old_hits[i];
            rend = hit.transform.GetComponent<Renderer>();

            if (rend)
            {
                bool matched = false;
                for (int j = 0; j < hits.Length; j++) {
                    if (hit.transform == hits[j].transform)
                        matched = true;
                }

                Shader[] rend_shaders = (Shader[])shaders[rend];

                // Change the material of all hit colliders
                // to use a transparent shader.
                if (matched == false) {
                    for (int j = 0; j < rend.materials.Length; j++) {
                        rend.materials[j].shader = rend_shaders[j];
                        Color tempColor = rend.materials[j].color;
                        tempColor.a = 1f;
                        rend.materials[j].color = tempColor;
                    }
                }
            }
        }
        old_hits = hits;
        // END RAYCAST TRANSPARENCY FUNCTION

        // keep within circle of player
        radius = baseRadius;
        // calculate player.transform.position x y and z
        Vector3 playerXZ = Vector3.zero;
        playerXZ.x += player.transform.position.x + Mathf.Cos(t) * radius;
        playerXZ.y += player.transform.position.y + y_axis_position;
        playerXZ.z += player.transform.position.z + Mathf.Sin(t) * radius;
        // lerp to position
        transform.position = Vector3.Lerp(transform.position, playerXZ, Time.deltaTime * lerpSpeed);
    }

    private void LateUpdate()
    {
        if (player == null) return;
        transform.LookAt(player.transform.position, Vector3.up);
    }


    //private bool CheckWall()
    //{
    //    Vector3 checker = transform.position;
    //    checker.y = player.transform.position.y;
    //    float dist = 4f;
    //    //Debug.DrawRay(player.transform.position, checker - player.transform.position, Color.yellow);
    //    
    //    if(Physics.Raycast(player.transform.position, checker - player.transform.position, out camhit, dist, ~mask)){
    //        
    //        return true;
    //    }
    //    return false;
    //    } 
}

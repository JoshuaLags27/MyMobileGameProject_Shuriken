using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Class is Responsible for the initial spawing of the via player input of the Shuriken
public class TouchNodeSpawn : MonoBehaviour
{
    public GameObject[] SpawnPrefab;
    public Transform[] SpawnNodes;
    [Space]
    public Vector3 MousePosition; 
    public Vector3 touchPosition;
    [Space]
    public float Offset_Z_MouseArea;
    public float Offset_Z_BallSpawn;
    [Space]
    public float ProjectileSpeed;
    [Space]
    public int Shuriken_Index;
    [Space]
    public bool Dragging;
    public bool SpawnObjectEnabled;
    

    // The Position Vector of the 3 coordinates on the camera
    private Vector3 offset_position;
    // This Marks the tagged gameObject in the 
    private GameObject MarkedGameObject;
    // The Position Relative to the spawn area of the Shurikens
    private Vector3 MarkedObjectPosition = new Vector3();


    // Update is called once per frame
    void Update()
    {

        // The Offset vector to transform spawned gameObject forward of the camera
        offset_position = new Vector3(0, 0, Offset_Z_MouseArea);

        RaycastHit hit, hit_Index;

        // The Position from the Ball to the MousePosition via Raycast
        Debug.DrawLine(MarkedObjectPosition, MousePosition + offset_position, Color.green);

        // The Position of the Ball Spawn Node, and the Offset of the Spawn Nodes
        Debug.DrawLine(SpawnNodes[Shuriken_Index].position, SpawnNodes[Shuriken_Index].position + new Vector3(0, 0, Offset_Z_BallSpawn),Color.blue);

        #region Mouse Input

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Vector3 - Updates Every frame the position of the Cursor 
            MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Physics.Raycast(MousePosition, transform.forward, out hit_Index))
            {
                if (hit_Index.transform.gameObject.name == "Default_Mode")
                {
                    Shuriken_Index = 0;
                    SpawnObjectEnabled = true;
                }

                else if (hit_Index.transform.gameObject.name == "Fire_Mode")
                {
                    Shuriken_Index = 1;
                    SpawnObjectEnabled = true;
                }

                else if (hit_Index.transform.gameObject.name == "Ice_Mode")
                {
                    Shuriken_Index = 2;
                    SpawnObjectEnabled = true;
                }
                else
                {
                    SpawnObjectEnabled = false;
                }
            }


            // Spawns the Shuriken on the Position of the Cursor or touch 
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (SpawnObjectEnabled)
                {
                    Instantiate(SpawnPrefab[Shuriken_Index], SpawnNodes[Shuriken_Index].position + new Vector3(0,0, Offset_Z_BallSpawn), Quaternion.identity);
                }

                Dragging = true;

                if (Physics.Raycast(MousePosition, transform.forward, out hit))
                {
                    //Assign the Marked GameObject Currently held via cursor
                    MarkedGameObject = hit.transform.gameObject;

                    // Get the area position vector of the Marked GameObject
                    MarkedObjectPosition = MarkedGameObject.transform.position;
                  
                    // Disable the Gravity of the GameObject rigidbody.
                    MarkedGameObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }

            // Drags the Shuroken in World Space
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (Dragging)
                {
                    if(MarkedGameObject && MarkedObjectPosition != null)
                    {

                        // The Actual Direction where the Ball flings onto
                        Debug.DrawLine(MarkedObjectPosition, MousePosition + offset_position, Color.green);

                    }
                }
            }

            // Enables Gravity In World Space
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Dragging = false;

                if (MarkedGameObject != null)
                {
                    // Throw in the Direction of the Cursor
                    MarkedGameObject.transform.GetComponent<Rigidbody>().AddForce(((MousePosition - MarkedObjectPosition) + offset_position) * ProjectileSpeed, ForceMode.Impulse);
                }

                MarkedGameObject = null;
            }
        }

        #endregion

        #region Mobile Input


        if (Application.platform == RuntimePlatform.Android)
        {

            /*
             * if (Input.touchCount > 0)
             * {
             * 
             * }
             */

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                if (Physics.Raycast(touchPosition, transform.forward, out hit_Index))
                {
                    if (hit_Index.transform.gameObject.name == "Default_Mode")
                    {
                        Shuriken_Index = 0;
                        SpawnObjectEnabled = true;
                    }

                    else if (hit_Index.transform.gameObject.name == "Fire_Mode")
                    {
                        Shuriken_Index = 1;
                        SpawnObjectEnabled = true;
                    }

                    else if (hit_Index.transform.gameObject.name == "Ice_Mode")
                    {
                        Shuriken_Index = 2;
                        SpawnObjectEnabled = true;
                    }
                    else
                    {
                        SpawnObjectEnabled = false;
                    }
                }
                

                if (touch.phase == TouchPhase.Began)
                {

                    if (SpawnObjectEnabled)
                    {
                        Instantiate(SpawnPrefab[Shuriken_Index], SpawnNodes[Shuriken_Index].position + new Vector3(0, 0, -5), Quaternion.identity);
                    }

                    Dragging = true; // Enable Dragging to true

                    if (Physics.Raycast(touchPosition, transform.forward, out hit))
                    {
                        //Assign the Marked GameObject Currently held via cursor
                        MarkedGameObject = hit.transform.gameObject;

                        // Get the area position vector of the Marked GameObject
                        MarkedObjectPosition = MarkedGameObject.transform.position;

                        // Disable the Gravity of the GameObject rigidbody.
                        MarkedGameObject.GetComponent<Rigidbody>().useGravity = false;

                    }

                }

                if (touch.phase == TouchPhase.Moved)
                {
                    if (Dragging)
                    {
                        if (MarkedGameObject && MarkedObjectPosition != null)
                        {
                            // The Actual Direction where the Ball flings onto
                            Debug.DrawLine(MarkedObjectPosition, touchPosition + offset_position, Color.green);
                        }
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    Dragging = false;

                    if (MarkedGameObject != null)
                    {
                        // Throw in the Direction of the Cursor
                        MarkedGameObject.transform.GetComponent<Rigidbody>().AddForce(((touchPosition - MarkedObjectPosition) + offset_position) * ProjectileSpeed, ForceMode.Impulse);
                        Debug.Log("ProjectileSpeed: " + ProjectileSpeed);
                    }

                    MarkedGameObject = null;
                }
            }
        }



        #endregion
    }
}

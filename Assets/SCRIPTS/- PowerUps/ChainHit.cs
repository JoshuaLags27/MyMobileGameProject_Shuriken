using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

// 2021-02-12  This Power up hits multiple targets, By Slowing down time while dragging fingers on multiple targets
public class ChainHit : MonoBehaviour
{

    // ----------------
    // PUBLIC VARIABLES
    // ----------------

    [Header("PowerUp Properties")]
    [Space]
    public float PowerUpOneMeter;
    public float PowerUpOneMeterMax;
    [Space]
    public Image PowerUpOneGauge;
    [Space]
    public bool PowerUpRefill;


    [Header("Local Properties")]
    [Space]
    public bool activate;                     // a bool variable that activates the powerup
    public bool release;                      // a boole variable for the powerup to take effect
    [Space]
    public float CrossMarkerDistance = -0.5f;
    public float objectSpeed;                 // This gameobjects velocity speed travel value
    [Range(0f,0.5f)] public float TimeSpeed;  // Value amount to control the timescale speed of the scene
    [Space]
    public GameObject RegisteredShurikenGameObject;    // The Object to be implemented with the powerup.
    [Space]
    public List<GameObject> EnemiesTagged = new List<GameObject>();   // A List of  of transforms for the gameObjects in the scene
    [Space]
    public List<GameObject> activeCrossMarkers = new List<GameObject>();         // all the game marker objects spawned in the scene to its gameObject.
    [Space]
    [Header(" Visual References ")]
    [Space]
    public GameObject CrossTagGameObject;    // The X Prefab with the Cross Mark Animation
    [Space]
    public AudioClip WoodhitSound;

    // ----------------
    // PRIVATE VARIABLES
    // ----------------
    Transform ObjectintoList;                // The Transform to Register the Currently tagged enemy to this variable

    GameObject crossTagGameObjectReference;

    Score score;

    GameplayFunctions gameplayFunctions;

    TouchGameplay touchGameplay;
    EditorGameplay editorGameplay;

    float destroyDistance = 0.0f;
    float destroyThreshold = 0.5f;

    private GameObject[] allEnemies;

    Vector3 cursorPosition, touchPosition;

    private void Start()
    {
        //PowerUpOneMeter = PowerUpOneMeterMax;
    }

    // Update is called once per frame
    void Update()
    {
        // -------------------------
        // REFERENCE CALL
        // -------------------------   
        touchGameplay = GameObject.FindObjectOfType<TouchGameplay>();

        editorGameplay = GameObject.FindObjectOfType<EditorGameplay>();

        gameplayFunctions = GameObject.FindObjectOfType<GameplayFunctions>();

       
        #region User Input

        if (activate == true)
        {
            Debug.Log("PowerUp Activated");

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {

                cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //---------------------
                // LMB - PRESSED
                //---------------------
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    InputTapped(cursorPosition);
                }
                //---------------------
                // LMB - HELD
                //---------------------
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    InputHeld(cursorPosition);
                }
                //---------------------
                // LMB - LIFTED
                //---------------------
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    InputReleased(touchPosition);
                }
            }

            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                    //---------------------
                    // FIRST FINGER - TAPPED
                    //---------------------
                    if (touch.phase == TouchPhase.Began)
                    {
                        InputTapped(touchPosition);
                    }
                    //---------------------
                    // FIRST FINGER - HELD
                    //---------------------
                    if (touch.phase == TouchPhase.Moved)
                    {
                        InputHeld(touchPosition);
                    }
                    //---------------------
                    // FIRST FINGER - RELEASED
                    //---------------------
                    if (touch.phase == TouchPhase.Ended)
                    {
                        InputReleased(touchPosition);
                    }
                }
            }

            #endregion



            // If RELEASE is set to TRUE, the REGISTERED GAMEOBJECT starts hitting multiple targets.
            if (release == true)
            {
                // SHURIKEN SPRITE BACK AT FULL ALPHA
                if (editorGameplay != null && editorGameplay.enabled == true && Application.platform == RuntimePlatform.WindowsEditor)
                {
                    editorGameplay.shurikenAlphaIndicator(editorGameplay.starAlphaNormal);
                }
                else if (touchGameplay != null && touchGameplay.enabled == true && Application.platform == RuntimePlatform.Android)
                {
                    touchGameplay.shurikenAlphaIndicator(touchGameplay.starAlphaNormal);
                }

                // IF THERE ARE ENEMY OBJECTS IN THE LIST THAT ARE REFERENCED AND ASSIGNED IN ANY AMOUNT
                if (EnemiesTagged.Count > 0)
                {
                    // REG GAMEOBJECT MOVES TOWARD THE SHURIKEN
                    RegisteredShurikenGameObject.transform.position = Vector3.MoveTowards
                    (
                        RegisteredShurikenGameObject.transform.position,
                        EnemiesTagged[0].transform.position,
                        objectSpeed * Time.deltaTime
                    );


                    // CALCULATE THE CURRENT DISTANCE BETWEEN THE REG'D GAMEOBJECT AND THE TAGGED ENEMY GAMEOBJECT
                    destroyDistance = Vector3.Distance(RegisteredShurikenGameObject.transform.position, EnemiesTagged[0].transform.position);


                    // MAKE ALL PRESENT CROSS MARKERS FOLLOW THEIR ASSIGNED ENEMIES
                    for (int i = 0; i < activeCrossMarkers.Count; i++)
                    {
                        activeCrossMarkers[i].transform.position = EnemiesTagged[i].transform.position + new Vector3(0, 0, CrossMarkerDistance);
                    }


                    // IF THE REGISTERED GAMEOBJECT IS NEAR THE ENEMY TAGGED OBJECTS
                    if (destroyDistance <= destroyThreshold)
                    {
                        // Add Score to the player
                        Scored();

                        // Play One Shot of the Sound
                        if (WoodhitSound != null)
                        {
                            this.GetComponent<AudioSource>().PlayOneShot(WoodhitSound);
                        }

                        // REMOVE THE TARGET GAMEOBJECT ON LIST INDEX -> 0
                        Destroy(EnemiesTagged[0].gameObject);
                        EnemiesTagged.Remove(EnemiesTagged[0]);

                        // REMOVE THE CROSS PREFAB ON LIST INDEX -> 0
                        Destroy(activeCrossMarkers[0].gameObject);
                        activeCrossMarkers.Remove(activeCrossMarkers[0]);
                    }

                }
                // IF THERE ARE NO ENEMIES ASSIGNED AND REFERENCE WITH NO AMOUNT IN THE LIST
                else if (EnemiesTagged.Count <= 0)
                {
                    // Enable Cooldown from the PowerUpActivator Class
                    EnableCooldown();

                    // If EditorGameplay is Present
                    if (editorGameplay != null)
                    {
                        // Enable its throw effect
                        editorGameplay.EnabledThrowEffect = true;
                    }
                    // If EditorGameplay is Present
                    if (touchGameplay != null)
                    {
                        // Enable its throw effect
                        touchGameplay.EnabledThrowEffect = true;
                    }

                    // Destroy the Registered Shuriken GameObject
                    Destroy(RegisteredShurikenGameObject);

                    timeFreeze(5, true);

                    DisablePowerUp();
                }
            }
        }

        PowerUpFunctions();

    }

    private void PowerUpFunctions()
    {
        // Sync the PU1 Gauge with the amount
        PowerUpOneGauge.fillAmount = PowerUpOneMeter / PowerUpOneMeterMax;

        if (Application.platform == RuntimePlatform.Android)
        {
            touchGameplay = GameObject.FindObjectOfType<TouchGameplay>();

            if (PowerUpOneMeter >= PowerUpOneMeterMax && touchGameplay.CurrentCooldown <= 0 && touchGameplay.enabled == true)
            {
                // Make the button interactable
                PowerUpOneGauge.gameObject.GetComponent<Button>().interactable = true;

                // The amount is locked to it's equivalent amount
                PowerUpOneMeter = PowerUpOneMeterMax;

                PowerUpRefill = false;
            }
        }
        
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            editorGameplay = GameObject.FindObjectOfType<EditorGameplay>();

            if (PowerUpOneMeter >= PowerUpOneMeterMax && editorGameplay.CurrentCooldown <= 0 && editorGameplay.enabled == true)
            {
                // Make the button interactable
                PowerUpOneGauge.gameObject.GetComponent<Button>().interactable = true;

                // The amount is locked to it's equivalent amount
                PowerUpOneMeter = PowerUpOneMeterMax;

                PowerUpRefill = false;
            }
        }

        // PowerUpActive is Set to True        
        if (PowerUpRefill == true)
        {
            // Refill the Gauge over time
            PowerUpOneMeter += Time.deltaTime;
        }
    }

    // Returns True if the Player Input is Within the Boundary
    bool InputInsideBoundary(Vector3 pressedPosition)
    {
        return pressedPosition.y < gameplayFunctions.topBoundaryAmount.position.y && pressedPosition.x > gameplayFunctions.topBoundaryAmount.position.x &&
        pressedPosition.x < gameplayFunctions.BottomBoundaryAmount.position.x && pressedPosition.y > gameplayFunctions.BottomBoundaryAmount.position.y;
    }

    bool InputOutSideBoundary(Vector3 pressedPosition)
    {
        return pressedPosition.y > gameplayFunctions.topBoundaryAmount.position.y && pressedPosition.x < gameplayFunctions.topBoundaryAmount.position.x &&
        pressedPosition.x > gameplayFunctions.BottomBoundaryAmount.position.x && pressedPosition.y < gameplayFunctions.BottomBoundaryAmount.position.y;
    }


    public void EnableCooldown()
    {
        // Enable the Cooldown from the PowerActivator GameObject
        PowerUpRefill = true;

        // Deactivate the interactive checkbox
        PowerUpOneGauge.gameObject.GetComponent<Button>().interactable = false;

        // Set back to its original color
        PowerUpOneGauge.color = Color.white;

        // Set the PowerUp Meter to zero
        PowerUpOneMeter = 0;

    }

    public void EnablePowerUp()
    {

        // Finds the Chainhit Powerup then activates it.
        activate = true;

        // If 'EnemyBreach' GameObject is present
        if (GameObject.FindObjectOfType<EnemyBreach>() != null)
        {
            // Access it and call and enable to true the 'activatePowerup'
            GameObject.FindObjectOfType<EnemyBreach>().activePowerup = true;
        }

        // Change the color of the cooldown image to green.
        PowerUpOneGauge.color = Color.green;

        // Disable the Throwing Physics from the Gameplay Functions
        GameObject.FindObjectOfType<GameplayFunctions>().PlatformCheckThrownEnabled(false);

        // Deactivate the interactive checkbox
        PowerUpOneGauge.gameObject.GetComponent<Button>().interactable = false;
    }


    void InputTapped(Vector3 pressedPosition)
    {
        if (InputInsideBoundary(pressedPosition))
        {
            Time.timeScale = TimeSpeed;
        }
    }
   
    void InputHeld(Vector3 pressedPosition)
    {
        RaycastHit hit, reg;
        
            // ASSIGN RAYCAST GAMEOBJECT TO "REGISTERED SHURIKEN GAMEOBJECT"
            if (RegisteredShurikenGameObject == null)
            {
                if (Physics.Raycast(pressedPosition, transform.forward, out reg))
                {
                    // ASSIGN THE RAYCAST GAMEOBJECT TO THE VARIABLE
                    RegisteredShurikenGameObject = reg.transform.gameObject;

                    RegisteredShurikenGameObject.GetComponent<SphereCollider>().enabled = false;
                    // DISABLE THE "HIT_TRIGGER" CLASS IN THAT GAMEOBJECT
                    RegisteredShurikenGameObject.GetComponent<ShurikenCollider>().Enable = false;
                }
            }

            // ------------------------------------------------------------------

            // -- REGISTER THE MARKEDOBJECTS (CROSS PREFAB) ON SCREEN TO MULTIHIT -- 
            if (Physics.Raycast(pressedPosition, transform.forward, out hit))
            {
                // If Raycast hits a gameObjects position
                if (hit.transform.gameObject.GetComponent<Enemy_Movement>() != null)
                {
                    // Assign the GameObject hit info to the Object Into List variable
                    ObjectintoList = hit.transform;

                    // MARKS AND ADDS TAGGED ENEMIES INTO THE LIST VARIABLE OF "Enemies Tagged"
                    if (!EnemiesTagged.Contains(ObjectintoList.gameObject))
                    {
                        // SPAWN THE X PREFAB TO THE TARGET
                        crossTagGameObjectReference = Instantiate(CrossTagGameObject, ObjectintoList.position + new Vector3(0, 0, CrossMarkerDistance), Quaternion.identity) as GameObject;

                        // Add the Hit GameObject to the "activeCrossMarkers" List
                        activeCrossMarkers.Add(crossTagGameObjectReference);

                        // Add the Hit GameObject to the "EnemiesTagged" List
                        EnemiesTagged.Add(ObjectintoList.gameObject);

                        // Unassign the GameObject assigned to the list 
                        ObjectintoList = null;
                    }
                }
            }

            // ------------------------------------------------------------------

            //  Assign every CrossMarker to the position of each cross marker
            for (int i = 0; i < activeCrossMarkers.Count; i++)
            {
                activeCrossMarkers[i].transform.position = EnemiesTagged[i].transform.position + new Vector3(0, 0, CrossMarkerDistance);
            }
        
    }

    void InputReleased(Vector3 pressedPosition)
    {
            // IF THERE ARE ENEMIES STORED IN THE ARRAY
            if (EnemiesTagged.Count > 0 && RegisteredShurikenGameObject != null)
            {
                // Set time back to normal
                Time.timeScale = 1.0f;
                // Stop time
                timeFreeze(0, false);
                // Release the Shuriken
                release = true;
            }

        // IF THERE ARE NO ENEMIES TAGGED, REMOVE AND DISABLE THIS POWERUP
        if (EnemiesTagged.Count == 0 && RegisteredShurikenGameObject != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Time.timeScale = 1.0f;

            Destroy(RegisteredShurikenGameObject);

            EnableCooldown();

            DisablePowerUp();

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (editorGameplay != null && editorGameplay.enabled == true)
                {
                    Debug.Log("Editor Gameplay is Present");
                    editorGameplay.EnabledThrowEffect = true;
                }
            }

            if (Application.platform == RuntimePlatform.Android)
            {
                if (touchGameplay != null && touchGameplay.enabled == true)
                {
                    Debug.Log("Touch Gameplay is Present");
                    touchGameplay.EnabledThrowEffect = true;
                }
            }
        }
    }
  
    // SIMULATES TIME BEING FROZEN BY MAIPULATING THE PROPERTIES OF THE SPAWNER AND ALL PRESENT ENEMIES IN THE SCENE 
    void timeFreeze(float EnemyMoveSpeed, bool spawnerEnabled)
    {
        // -----------------------
        // STOP THE MOTION OF TAGGED ENEMIES
        // -----------------------

        for (int i = 0; i < EnemiesTagged.Count; i++)
        {
            Destroy(EnemiesTagged[i].GetComponent<Enemy_Movement>());
        }

        /*
        foreach (Enemy_Movement enemies in GameObject.FindObjectsOfType<Enemy_Movement>())
        {
            enemies.GetComponent<Enemy_Movement>().speed = EnemyMoveSpeed;
        }
        */

        // -----------------------
        // STOP THE SPAWNING FROM OCCURING
        // -----------------------

        GameObject.FindObjectOfType<EnemySpawner_Falling>().enabled = spawnerEnabled;
    }
    
    // DEACTIVATES THE POWER UP
    void DisablePowerUp()
    {
        activate = false;
        release = false;

        if(GameObject.FindObjectOfType<EnemyBreach>() != null)
        {
            GameObject.FindObjectOfType<EnemyBreach>().activePowerup = false;
        }
    }

    // GIVE SCORE TO THE PLAYER. 
    public void Scored()
    {
        // Call the GameObject Score and Get it's Score Script Component
        if (GameObject.FindObjectOfType<Score>() != null)
        {
            score = GameObject.FindObjectOfType<Score>();
        }

        // Increment the Score to 1
        score.AddScore(1);
    }
    
}


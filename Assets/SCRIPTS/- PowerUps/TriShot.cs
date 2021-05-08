using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class TriShot : MonoBehaviour
{
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
    public bool thrownState;                        // a bool variable that activates the powerup
    public bool releaseState;                       // a boole variable for the powerup to take effect
    public bool usageLock;                          // to lock the powerup during its Cooldown phase
    [Space]
    public GameObject registeredShurikenGameObject; // Shuriken To Be Registered;
    [Space]
    public GameObject smoke;
    public GameObject smokeLoop;
    GameObject smokeLoopClone;
    [Space]
    public float disperseSpeed = 5.0f;
    public float disperseReleaseTime = 0.025f;
    [Space]
    public GameObject shurikenGameObject;           // The GameObject to be utilized when split
    [Range(0f,1f)]
    public float timeSpeed;                         // The Speed to Slow down upon using the powerup
    [Space]
    private Vector3 mousePosition, touchPosition;

    TouchGameplay touchGameplay;
    EditorGameplay editorGameplay;

    // Update is called once per frame
    void Update()
    {
        PowerUpGaugeUpdate();

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Mouse Position
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (thrownState == true)
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    buttonHeld1stPhase(mousePosition);
                }

                if (Input.GetKeyUp(KeyCode.Mouse0) && !editorGameplay.IsPointerOverUIObject())
                {
                    buttonReleased();
                }
            }
            else if (releaseState == true && thrownState == false)
            {
                // To Slow down time of the powerup
                if (Input.GetKey(KeyCode.Mouse0) && !editorGameplay.IsPointerOverUIObject())
                {
                    buttonHeld2ndPhase();
                }

                // Enable Cooldown if the Projectile is out of bounds
                if (registeredShurikenGameObject == null)
                {
                    EnableCooldown();
                    releaseState = false;
                }
            }
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            // Touch Position

            Touch touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (thrownState == true)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    buttonHeld1stPhase(touchPosition);
                }
                
                if (touch.phase == TouchPhase.Ended && !touchGameplay.IsPointerOverUIObject())
                {
                    buttonReleased();
                }
               
            }
            else if (releaseState == true && thrownState == false)
            {
               
                if (touch.phase == TouchPhase.Began && !touchGameplay.IsPointerOverUIObject())
                {
                    if (usageLock == true)
                    {
                        buttonHeld2ndPhase();
                    }
                }
                
                // Enable Cooldown if the Projectile is Missing
                if (registeredShurikenGameObject == null)
                {
                    EnableCooldown();
                    releaseState = false;
                }

            }

        }
    }

    private void PowerUpGaugeUpdate()
    {
        // Sync the PU1 Gauge with the amount
        PowerUpOneGauge.fillAmount = PowerUpOneMeter / PowerUpOneMeterMax;

        // PowerUpActive is Set to True        
        if (PowerUpRefill == true)
        {
            // Refill the Gauge over time
            PowerUpOneMeter += Time.deltaTime;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            touchGameplay = GameObject.FindObjectOfType<TouchGameplay>();

            if (PowerUpOneMeter >= PowerUpOneMeterMax && touchGameplay.CurrentCooldown <= 0 && touchGameplay.enabled == true)
            {
                // Make the button interactable
                PowerUpOneGauge.gameObject.GetComponent<Button>().interactable = true;
                // The amount is locked to it's equivalent amount
                PowerUpOneMeter = PowerUpOneMeterMax; 
                // Disable the Refill 
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
                // Disable the Refill 
                PowerUpRefill = false;
            }
        }
    }


    private void buttonHeld1stPhase(Vector3 pressedPosition)
    {
        RaycastHit registry;

        if (registeredShurikenGameObject == null)
        {
            // Assign the Regsitered Shuriken GameObject
            if (Physics.Raycast(pressedPosition, transform.forward, out registry))
            {
                registeredShurikenGameObject = registry.transform.gameObject;
            }

            if (smoke != null && registeredShurikenGameObject != null)
            {
                smokeLoopClone = Instantiate(smokeLoop, registeredShurikenGameObject.transform.position, Quaternion.identity) as GameObject;

                smokeLoopClone.transform.parent = registeredShurikenGameObject.transform;
            }

        }
    }

    private void buttonHeld2ndPhase()
    {
        if (usageLock == true && registeredShurikenGameObject != null)
        {
            // Invoke the Shuriken Gameplay Physics
            StartCoroutine(SprayShot()); 
            // Set time Back to Normal
           
            // Lock this condition for the PowerUp to be Executed Only once
            usageLock = false;
            // Enable the cooldown of powerup
            EnableCooldown();
            // Disable the Release State
            releaseState = false;
        }
        
    }

    private void buttonReleased()
    {
        thrownState = false; // disable the thrownState boolean 
        usageLock = true;
        releaseState = true; // Enable the releaseState boolean
    }
    
    // Throw shurikens via upward direction with the x value as the direction as an argument
    void throwMiniShurikens(float directionToThrow)
    {
        GameObject shurikens = Instantiate(shurikenGameObject, registeredShurikenGameObject.transform.position, Quaternion.identity) as GameObject;
        shurikens.GetComponent<Rigidbody>().AddForce(new Vector3(directionToThrow * disperseSpeed, 1.5f * disperseSpeed, 0), ForceMode.Impulse);
    }
    
    // Spawn multiple shurikens in a Upward direction in all angles
    IEnumerator SprayShot()
    {
        // Destroy this gameObject, then spawn and throw the other 3.
        registeredShurikenGameObject.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(registeredShurikenGameObject.gameObject.GetComponent<SphereCollider>());

        throwMiniShurikens(-1.5f);
        throwMiniShurikens( 1.5f);
      
        yield return new WaitForSecondsRealtime(disperseReleaseTime);

        throwMiniShurikens(-1.25f);
        throwMiniShurikens( 1.25f);

        yield return new WaitForSecondsRealtime(disperseReleaseTime);

        throwMiniShurikens(-1f);
        throwMiniShurikens( 1f);;
      
        yield return new WaitForSecondsRealtime(disperseReleaseTime);

        throwMiniShurikens(-0.75f);
        throwMiniShurikens( 0.75f);
       
        yield return new WaitForSecondsRealtime(disperseReleaseTime);

        throwMiniShurikens(-0.5f);
        throwMiniShurikens( 0.5f);

        yield return new WaitForSecondsRealtime(disperseReleaseTime);

        throwMiniShurikens(-0.25f);
        throwMiniShurikens( 0.25f);

         yield return new WaitForSecondsRealtime(disperseReleaseTime);

        throwMiniShurikens( 0f);

        registeredShurikenGameObject.transform.GetChild(1).GetComponent<ParticleSystem>().Stop();
        // Destroy the second child of the registered shuriken gameObject
        Destroy(registeredShurikenGameObject.transform.GetChild(0).gameObject);

        Destroy(registeredShurikenGameObject,2.5f);

        registeredShurikenGameObject = null;
     
    }
    
    // Initialize the Cool Down to the 'PowerUp' Function 
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
    
    // Activate the 'PowerUp' function 
    public void EnablePowerUp()
    {
        Debug.Log("Powerup enabled");

        // Finds the Chainhit Powerup then activates it.
        thrownState = true;
        
        // Change the color of the cooldown image to green.
        PowerUpOneGauge.color = Color.green;

        // Deactivate the interactive checkbox
        PowerUpOneGauge.gameObject.GetComponent<Button>().interactable = false;
    }
    
   

}

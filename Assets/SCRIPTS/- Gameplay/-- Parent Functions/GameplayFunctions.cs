using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameplayFunctions : MonoBehaviour
{
    //---------------------
    // PUBLIC VARIABLES
    //---------------------
    public bool EnabledThrowEffect;     // For the player to throw the shuriken
    public bool Cooled_down;            // Cool down for the Shuriken to be re-used again
    [Space]
    public GameObject ProjectileObject; // The Projectile gameobject to be thrown
    [Space]
    [Space]
    public Vector3 cursorPosition;      // The Current position of the mouse Cursor.
    public Vector3 touchPosition;       // The Current position of the fingers touch.
    [Space]
    public Vector3 registerSpawnPos;    // The Starting registered input of the left mouseButton.
    [Space]
    public float offset_z;              // The Offset spawn position of the cursorPosition.
    [Space]
    [Header("Spawned Shuriken Properties")]
    [Space]

    public float followSpeed = 5.0f;        // The speed in which the followed gameObject is set to move towards the mouseCursor
    public float throwSpeed = 5.0f;         // The speed in which the projectile will inherit when flinged off

    public float starAlphaTransluscent = 0.5f;
    public float starAlphaNormal = 1.0f;

    [Space]
    [Header("Throw Interval Indicator")]
    [Space]

    public Image CooldownCircle;
    public Image CooldownHighlightLogo;
    public float CurrentCooldown;

    [Space]
    [Header("Shuriken Spawn Boundary")]
    [Space]

    public Transform topBoundaryAmount;
    public Transform BottomBoundaryAmount;

    [Space]
    //---------------------
    // PRIVATE / HIDDEN VARIABLES
    //---------------------
    
    [HideInInspector] public Vector3 SpawnBoundaryHeight;       // The Height Limit of where the player can spawn shurikens

    public GameObject MarkedGameObject;       // The GameObject to be marked from the raycast function

    [HideInInspector] public GameObject markedGameObjectChild;

    [HideInInspector] public float CurrentCooldownRegistry;
    [HideInInspector] public Color colorOfLogo;

    [HideInInspector] public Vector3 DirectionOftheThrownShuriken;

    GameManager gameManager;

    // THE SHURIKEN ASSIGNED WITH IT'S UI SPRITE FOR PLAYER INDICATION
    public void shurikenAlphaIndicator(float alphaValue)
    {
        if(markedGameObjectChild != null)
        {
            markedGameObjectChild = MarkedGameObject.transform.GetChild(0).gameObject;
            markedGameObjectChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, alphaValue);
        }
    }

    // THE SHURIKEN UI COOL DOWN STATE 
    public void LogoTransluscentEnabled(bool enabled)
    {
        colorOfLogo = CooldownHighlightLogo.color;

        if (enabled == true)
        {
            colorOfLogo.a = 0.5f;
        }
        else if (enabled == false)
        {
            colorOfLogo.a = 1.0f;
        }

        CooldownHighlightLogo.color = colorOfLogo;
    }

    // TO DETECT A UI IN THE SCREEN TO BE IGNORED BY THE TOUCH AND THE MOUSE
    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    // CHECKS THE PLATFORM, TO ENABLE OR DISABLE THE THROW PHYSICS
    public void PlatformCheckThrownEnabled(bool throwMechanic)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            GameObject.FindObjectOfType<EditorGameplay>().EnabledThrowEffect = throwMechanic;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            GameObject.FindObjectOfType<TouchGameplay>().EnabledThrowEffect = throwMechanic;
        }
    }

    // ----------- Gameplay Functions ----------- 

    public void inputPressed(Vector3 controlVariable)
    {
        RaycastHit hit;

        if (EventSystem.current.IsPointerOverGameObject())
        { return; }

        // Assign the clicked area as the registerSpawnPos, the Starting point of the vector
        registerSpawnPos = controlVariable;
         
        // Disable the Logo Transluscent
        LogoTransluscentEnabled(true);

        // If the Cooldown is finished.
        if (Cooled_down == true)
        {
            // Spawn a Shuriken
            if (controlVariable.y < topBoundaryAmount.position.y)
            {
                Instantiate(ProjectileObject, registerSpawnPos + new Vector3(0, 0, offset_z), Quaternion.identity);
            }
        }

        if (Physics.Raycast(controlVariable, transform.forward, out hit))
        {
            // Assign Mouse Clicked Object to "MarkedGameObject" variable
            if(hit.transform.gameObject.tag == "Shuriken")
            {
                MarkedGameObject = hit.transform.gameObject;

                Debug.Log("Hit Object Registered: " + hit.transform.name);
            }
            
            shurikenAlphaIndicator(starAlphaTransluscent);
        }
    }

    public void inputHold(Vector3 controlVariable)
    {
        // show a line of the registerSpawnPos as the starting point and the endpoint being the cursorPosition.
        Debug.DrawLine(registerSpawnPos + new Vector3(0, 0, offset_z), controlVariable + new Vector3(0, 0, offset_z), Color.green);
        
        // Disable the Sphere Collider when Projectile is held down
        if (MarkedGameObject != null && MarkedGameObject.tag == "Shuriken")
        {
            MarkedGameObject.GetComponent<SphereCollider>().isTrigger = true;

            // CLAMP THE MOUSE CURSOR AREA WITHIN THE BOUNDARY
            controlVariable.y = Mathf.Clamp(controlVariable.y, BottomBoundaryAmount.position.y, topBoundaryAmount.position.y);
            controlVariable.x = Mathf.Clamp(controlVariable.x, topBoundaryAmount.position.x, BottomBoundaryAmount.position.x);

            // Move the Marked GameObject Shuriken on the position of the Cursor
            MarkedGameObject.transform.position = Vector3.MoveTowards(MarkedGameObject.transform.position, controlVariable + new Vector3(0, 0, offset_z), followSpeed * Time.deltaTime);
        }
    }

    public void inputReleased(Vector3 controlVariable)
    {
        // If A MarkedGameObject is present in the cursor being held down
        if (MarkedGameObject != null)
        {
            shurikenAlphaIndicator(starAlphaNormal);
            
            // Throws Shurien GameObject North and West
            if (controlVariable.y > topBoundaryAmount.position.y || controlVariable.x < topBoundaryAmount.position.x)
            {
                ThrowAndReleaseShuriken(controlVariable);
            }
            else if (controlVariable.x > BottomBoundaryAmount.position.x || controlVariable.y < BottomBoundaryAmount.position.y)
            {
                ThrowAndReleaseShuriken(controlVariable);
            }
            else
            {
                Destroy(MarkedGameObject);
                MarkedGameObject = null;
            }

        }
    }

    // ------------------------------------------  


    // This Function Throw, Unnasigns the Registered Shuriken GameObject to it's Input Position.
    void ThrowAndReleaseShuriken(Vector3 controlVariable)
    {
        // Call the Game Manager, Then increment by One it's Shuriken Thrown variables
        gameManager = GameObject.FindObjectOfType<GameManager>();

        // Calls the GameManager to count the amount of thrown shurikens
        gameManager.shurikensThrown += 1; 


        DirectionOftheThrownShuriken = ((controlVariable - MarkedGameObject.transform.position) + new Vector3(0, 0, offset_z));

        // Throw the Projectile in the direction of the Cursor on screen
        MarkedGameObject.transform.GetComponent<Rigidbody>().AddForce(DirectionOftheThrownShuriken * throwSpeed, ForceMode.Impulse);

        //Debug.Log("The Direction Thrown is at Vector: " + DirectionOftheThrownShuriken);

        // Enable the Sphere Collider when Projectile is released
        MarkedGameObject.GetComponent<SphereCollider>().isTrigger = false;

        // Unmarked the thrown gameObject;
        MarkedGameObject = null;

        // Disable the Cooldown
        Cooled_down = false;

        // Reset the Cooldown of the Spawning of Shurikens
        CurrentCooldown = CurrentCooldownRegistry;

        // Enable/Highlight the Cooldown UI
        CooldownCircle.enabled = true;
    }
}

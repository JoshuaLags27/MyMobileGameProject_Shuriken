using UnityEngine;

public class MenuFunctions : MonoBehaviour
{
    public GameObject MenuShuriken;
    public GameObject menuWheel;
    [Space]
    public GameObject registeredGameObject;
    public GameObject registeredGameObjectsChild;
    [Space]
    public Vector3 spawnDistance;
    public Vector3 spawnSizeSet;
    [Space]
    [HideInInspector] public Vector3 mousePosition;
    [HideInInspector] public Vector3 touchPosition;
    [Space]
    public float throwPower;
    public float objectDistanceThrowThreshold = 2.5f;
    public float objectsDistance;
    [Space]
    public bool fadeSwitch = false;

    protected RaycastHit hitReg, areaReg;

    public Vector3 directionOfProjectile;

    public void menuControl_Down(RaycastHit hitReg, RaycastHit areaReg, Vector3 PressedPosition)
    {
        // DETECT THE SPAWN AREA GAMEOBJECT TO SPAWN SHURIKEN
        if (Physics.Raycast(PressedPosition, transform.forward, out areaReg))
        {
            // TO FADE OUT THE TUTORIAL MESSAGE
            // GameObject.Find("INSTRUCTIONS").transform.GetComponent<Animator>().SetTrigger("continue");

            if (areaReg.transform.gameObject.name == "SpawnArea")
            {

                // SET STAR'S SIZE
                MenuShuriken.gameObject.GetComponent<Transform>().localScale = spawnSizeSet;
                // SPAWN THE STAR
                Instantiate(MenuShuriken, areaReg.transform.position + new Vector3(0f, 0f,-0.10f), Quaternion.identity);
            }
        }
        // DETECT THE SHURIKEN SPAWNED TO ASSIGN TO GET IT'S CHILD COMPONENT
        if (Physics.Raycast(PressedPosition, transform.forward, out hitReg))
        {
            if (hitReg.transform.gameObject.tag == "Shuriken")
            {
                registeredGameObject = hitReg.transform.gameObject;
                registeredGameObjectsChild = hitReg.transform.GetChild(0).gameObject;

                Debug.Log(hitReg.transform.gameObject.name + " gameObject was registered");
            }
        }
    }

    protected void menuControl_Hold(Vector3 PressedPosition)
    {
        // IF 2 GAMEOBJECTS ARE NOT VACANT, SPIN THE WHEEL
        if (registeredGameObject != null)
        {
            // CALCULATE THE DISTANCE BETWEEN THE STAR AND MOUSEPOSITION
            cursorObjectDistance();
            
            // ASSIGN THE DISTANCE OF THE SHURIKEN GAMOBJECT 
            objectsDistance = Vector3.Distance(registeredGameObject.gameObject.transform.position, PressedPosition + spawnDistance);
            
            Debug.DrawLine(registeredGameObject.gameObject.transform.position, PressedPosition + spawnDistance);
        }
    }

    protected void menuControl_Up(Vector3 PressedPosition)
    {
        if (registeredGameObject != null && objectsDistance <= objectDistanceThrowThreshold)
        {
            // THEN DESTROY THE GAMEOBJECT
            Destroy(registeredGameObject);
            // EMPTY THE STAR GAMEOBJECT
            registeredGameObject = null;
            // EMPTY THE STAR GAMEOBJECT'S CHILD
            registeredGameObjectsChild = null;
        }
        else if(registeredGameObject != null && objectsDistance >= objectDistanceThrowThreshold)
        {
            directionOfProjectile = ((PressedPosition - registeredGameObject.transform.position) + spawnDistance);
            // THROW THE STAR TOWARDS THE DIRECTION OF THE MOUSE POSITION
            registeredGameObject.transform.GetComponent<Rigidbody>().AddForce(directionOfProjectile * throwPower, ForceMode.Impulse);
            // EMPTY THE STAR GAMEOBJECT
            registeredGameObject = null;
            // EMPTY THE STAR GAMEOBJECT'S CHILD
            registeredGameObjectsChild = null;
        }
    }

    // Calculates the Distance of the Spawn Circle to the distance of users Held Fingers or the FirstClick button
    void cursorObjectDistance()
    {
        if(registeredGameObject != null)
        {
            if (objectsDistance <= objectDistanceThrowThreshold)
            {
                registeredGameObjectsChild.GetComponent<Spinning>().rotSpeed = 200.0f;
            }
            else if (objectsDistance >= objectDistanceThrowThreshold)
            {
                registeredGameObjectsChild.GetComponent<Spinning>().rotSpeed = 800.0f;
            }
        }
    }

}

using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuShurikenCollider : MonoBehaviour
{
    public string[] objectNames;
    [Space]
    public int[] sceneIndex;
    [Space]
    public int hitIndex;
    public bool destroyAndTransition;
    [Space]
    public float timeToTransition = 0.5f;
    public float forceAmount;
    public AudioClip WoodhitSound;
    public GameObject InsertShurikenImage;
    [Space]
    [Header("Shuriken Physics Properties")]
    [Space]

    Vector3 directionOfProjectile;

    private void Update()
    {
        if(destroyAndTransition == true)
        {
            switch (hitIndex)
            {
                case 1:
                    StartCoroutine(SceneTransitionTo(sceneIndex[0]));
                    destroyAndTransition = false;
                    break;
                case 2:
                    StartCoroutine(SceneTransitionTo(sceneIndex[1]));
                    destroyAndTransition = false;
                    break;
                case 3:
                    StartCoroutine(SceneTransitionTo(sceneIndex[2]));
                    destroyAndTransition = false;
                    break;
                case 4:
                    StartCoroutine(OnApplicationExit());
                    destroyAndTransition = false;
                    break;
            }
        }
    }

    
   IEnumerator OnApplicationExit()
   {
       Debug.Log("Application Exiting");
       yield return new WaitForSeconds(timeToTransition);
       Application.Quit();
   }

   IEnumerator SceneTransitionTo(int SceneIndex)
   {
       Debug.Log("Countdown countdown begins");
       yield return new WaitForSeconds(timeToTransition);

       Debug.Log("Transitioning Now!");
       SceneManager.LoadScene(SceneIndex);
   }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == objectNames[0]) // PLAY
        {
            Debug.Log("Shuriken Thrown at " + objectNames[0]);
            hitIndex = sceneIndex[0]; // 1
            destroyAndTransition = true;
           
        }
        else if (other.gameObject.name == objectNames[1]) // OPTION
        {
            Debug.Log("Shuriken Thrown at " + objectNames[1]);
            hitIndex = sceneIndex[1]; // 2
            destroyAndTransition = true;
           
        }
        else if (other.gameObject.name == objectNames[2]) // LEVELS
        {
            Debug.Log("Shuriken Thrown at " + objectNames[2]);
            hitIndex = sceneIndex[2]; // 3
            destroyAndTransition = true;
        }
        else if (other.gameObject.name == objectNames[3]) // EXIT
        {
            Debug.Log("Shuriken Thrown at " + objectNames[3]);
            hitIndex = sceneIndex[3]; // 4
            destroyAndTransition = true;
        }


        ShurikenPhysicsCollision(other);
    }

    // Imitate the Shuriken Physics in the Main Game
    public void ShurikenPhysicsCollision(Collision col)
    {
        // Play the Wood Hit SOund A Single Time
        col.gameObject.GetComponent<AudioSource>().PlayOneShot(WoodhitSound);
        // Disable the other gameObjectsgravitys
        col.gameObject.GetComponent<Rigidbody>().useGravity = true;

        // Enable the Trigger of both gameObject
        col.gameObject.GetComponent<SphereCollider>().isTrigger = true;
        this.gameObject.GetComponent<SphereCollider>().isTrigger = true;

        // Reference the GameplayFunction to Grab the Public Vector field/variable for the throw direction reference
        directionOfProjectile = (GameObject.FindObjectOfType<MenuFunctions>().directionOfProjectile) + new Vector3(0f, 0f, 2.5f);

        // Push the Other GameObject Upon Contact of Collision of this current GameObject
        col.gameObject.GetComponent<Rigidbody>().AddForce(directionOfProjectile * forceAmount, ForceMode.Impulse);

        // Assign this GameObject as Child to the Other GameObject
        gameObject.transform.parent = col.gameObject.transform;
        
        InsertShurikenImage = Instantiate(InsertShurikenImage, gameObject.transform.position, gameObject.transform.rotation) as GameObject;

        Destroy(gameObject.transform.GetChild(0).gameObject);

        InsertShurikenImage.transform.parent = col.gameObject.transform;
        InsertShurikenImage.transform.LookAt(col.gameObject.transform);
        InsertShurikenImage.transform.Rotate(0, 0, 80);


        // Stop rotatig this gameObject
        this.gameObject.transform.GetChild(0).GetComponent<Spinning>().rotSpeed = 0;

        // Destroy the Physics Component in this Shuriken GameObject
        Destroy(this.gameObject.GetComponent<Rigidbody>());
    }

    


}

using UnityEngine;

// (JL - 3/16/2021) Script for the Shuriken Behaviour
public class ShurikenCollider : MonoBehaviour
{
    public bool Enable;

    public string targetTag;
    public string targetName;
    [Space]
    public float forceAmount;
    [Space]
    public AudioClip WoodhitSound;
    [Space]
   // public GameObject collidedGameObject;
    [Space]
    public GameObject InsertShurikenImage;
  

    Score score;

    TouchGameplay touchGameplay;
    EditorGameplay editorGameplay;

    GameManager gameManager;

    Vector3 directionOfProjectile;
    
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision other)
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.shurikensHit += 1;

         if (Enable == true)
         {
            if(other.gameObject.tag == targetTag)
            {
                Debug.Log("TargetName Detected");

                #region Default_Shuriken

                // If Score GameObject is Present
                if (GameObject.FindObjectOfType<Score>() != null)
                {
                    score = GameObject.FindObjectOfType<Score>();
                    // Score + 1
                    score.Base_Score += 1;
                }

                // ComboSystem + 1
                if (GameObject.FindObjectOfType<ComboSystem>() != null)
                {
                    GameObject.FindObjectOfType<ComboSystem>().hitDetect();
                }

                // Reset the Cooldown Upon hitting an Object, Detects if Either the TouchGameplay or the EditorGameplay is active.
                if (GameObject.FindObjectOfType<TouchGameplay>() != null && GameObject.FindObjectOfType<TouchGameplay>().enabled == true && Application.platform == RuntimePlatform.Android)
                {
                    touchGameplay = GameObject.FindObjectOfType<TouchGameplay>();
                    touchGameplay.CurrentCooldown = 0;
                }
                else if (GameObject.FindObjectOfType<EditorGameplay>() != null && GameObject.FindObjectOfType<EditorGameplay>().enabled == true && Application.platform == RuntimePlatform.WindowsEditor)
                {
                    editorGameplay = GameObject.FindObjectOfType<EditorGameplay>();
                    editorGameplay.CurrentCooldown = 0;
                }

                #endregion

                ShurikenPhysicsOnWood(other);
            }
         }
         
    }


    private void ShurikenPhysicsOnWood(Collision other)
    {
        other.gameObject.GetComponent<Enemy_Movement>().speed = 0;
        // Play the Wood Hit SOund A Single Time
        other.gameObject.GetComponent<AudioSource>().PlayOneShot(WoodhitSound);
        // Disable the other gameObjectsgravitys
        other.gameObject.GetComponent<Rigidbody>().useGravity = true;

        // Destroy the The Parent GameObject Physics
        Destroy(other.gameObject.GetComponent<Enemy_Movement>());

        // Enable the Trigger of both gameObject
        Destroy(other.gameObject.GetComponent<SphereCollider>());
        Destroy(this.gameObject.GetComponent<SphereCollider>());

        // Reference the GameplayFunction to Grab the Public Vector field/variable for the throw direction reference
        directionOfProjectile = (GameObject.FindObjectOfType<GameplayFunctions>().DirectionOftheThrownShuriken) + new Vector3(0f,0f,2.5f);

        // Push the Other GameObject Upon Contact of Collision of this current GameObject
        other.gameObject.GetComponent<Rigidbody>().AddForce(directionOfProjectile * forceAmount , ForceMode.Impulse);

        // Assign this GameObject as Child to the Other GameObject
        gameObject.transform.parent = other.gameObject.transform;

        InsertShurikenImage = Instantiate(InsertShurikenImage, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        Destroy(gameObject);

        InsertShurikenImage.transform.parent = other.gameObject.transform; //
        InsertShurikenImage.transform.LookAt(other.gameObject.transform);  // Look at the Shuriken GameObject from the Y-axis
        InsertShurikenImage.transform.Rotate(0, 0, 80);                    // Rotate the Shuriken To the Camera


        // Set to 0 the rotation of gameObject's rotation
        this.gameObject.transform.GetChild(0).GetComponent<Spinning>().rotSpeed = 0;

        // Destroy the Physics Component in this Shuriken GameObject
        Destroy(this.gameObject.GetComponent<Rigidbody>());
    }

 

}
   

        


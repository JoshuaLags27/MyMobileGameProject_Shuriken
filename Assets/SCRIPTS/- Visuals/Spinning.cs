using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float rotSpeed = 3.5f;
    [Space]
    public bool rot_clockWise = true;
    [Space]
    public bool BeginSpinning;
    

    // Start is called before the first frame update
    void Awake()
    {
        BeginSpinning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (BeginSpinning)
        {
            if(rot_clockWise)
            {
                this.gameObject.transform.Rotate(0, 0, -rotSpeed * Time.smoothDeltaTime);
            }
            else
            {
                this.gameObject.transform.Rotate(0, 0, rotSpeed * Time.smoothDeltaTime);
            }
        }
          
    }

    private void OnBecameInvisible()
    {
        rotSpeed = 0.0f;
    }
}

using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Update()
    { 
        transform.position += transform.TransformDirection(Vector3.down) * speed * Time.deltaTime;
    }
    
}

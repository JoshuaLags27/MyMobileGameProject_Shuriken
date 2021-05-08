using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowDirection : MonoBehaviour
{

    public float HorizontalThrowStrength;  // 
    public float VerticalThrowStrength;    // 

    public bool LeftOrRight; // 
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void LeftThrow()
    {
        // Get the Rigidbody from this gameObject
        // rb = GetComponent<Rigidbody>();
        
        // Throw the projectile to the right side
        rb.AddForce(new Vector3(-1 * HorizontalThrowStrength, 1 * VerticalThrowStrength, 0), ForceMode.Impulse);
    }

    public void RightThrow()
    {
        // Get the Rigidbody from this gameObject
        // rb = GetComponent<Rigidbody>();

        // Throw the projectile to the right side
        rb.AddForce(new Vector3(1 * HorizontalThrowStrength, 1 * VerticalThrowStrength, 0), ForceMode.Impulse);
    }


}

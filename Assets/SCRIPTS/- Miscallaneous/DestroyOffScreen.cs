using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

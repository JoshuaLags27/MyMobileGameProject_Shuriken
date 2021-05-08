using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyGameObject : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}

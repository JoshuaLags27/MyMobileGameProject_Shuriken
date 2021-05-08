using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SomeClass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<BadGuy> badguys = new List<BadGuy>();

        badguys.Add(new BadGuy("Joshua", 50));
        badguys.Add(new BadGuy("Agreth", 30));
        badguys.Add(new BadGuy("Zouma", 5));

        badguys.Sort();

        foreach (BadGuy guy in badguys)
        {
            print(guy.name + " " + guy.power);
        }

        badguys.Clear();
    }

  
}

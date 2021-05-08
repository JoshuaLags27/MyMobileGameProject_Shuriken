 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_SideThrow : MonoBehaviour
{
    public GameObject[] RandomEnemies; // Objects to be thrown From the left and right spawner 
    [Space]
    public Transform[] SpawnAreas;  // The Spawn area of throwing oppositions to the screen
    [Space]
    public float SpawnInterval;   // The Spawning Intervals for every object thrown.
    [Space]
    public float LeftSpawnUp;
    public float LeftSpawnDown;
    [Space]
    public float RightSpawnUp;
    public float RightSpawnDown;
    [Space]
    public int ActiveSpawnIndex; // The Currently active spawn index, that was randomized

    private float SpawnIntervalRegistry;      // Assign the Current Spawn interval amount to the SpawnIntervalRegistry for reference
    private Vector3 RandomizedSpawnAreaLeft;    
    private Vector3 RandomizedSpawnAreaRight;

    private GameObject RegisterGameObject;
    // Start is called before the first frame update
    void Start()
    {
        // Assign the Current Spawn interval amount to the SpawnIntervalRegistry for reference
        SpawnIntervalRegistry = SpawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // Randomize the Index In Every Spawn
        SpawnInterval -= Time.deltaTime;

        if(SpawnInterval <= 0)
        {
            // Randomize a number from 0 to 1
            ActiveSpawnIndex = Random.Range(0, SpawnAreas.Length + 1);
            
            switch(ActiveSpawnIndex)
            {
                case 1:

                    // Randomize the Spawn Area from the origin to its up and down axis
                    RandomizedSpawnAreaLeft = new Vector3 (0,Random.Range(LeftSpawnUp, LeftSpawnDown),0);

                    RegisterGameObject = Instantiate(RandomEnemies[0], SpawnAreas[0].position + RandomizedSpawnAreaLeft, Quaternion.identity) as GameObject;

                    RegisterGameObject.GetComponent<EnemyThrowDirection>().RightThrow();

                    RegisterGameObject = null;

                    Debug.Log("Spawning Object from Left");

                    break;

                case 2:

                    // Randomize the Spawn Area from the origin to its up and down axis
                    RandomizedSpawnAreaRight = new Vector3(0, Random.Range(RightSpawnUp, RightSpawnDown), 0);

                    RegisterGameObject = Instantiate(RandomEnemies[0], SpawnAreas[1].position + RandomizedSpawnAreaRight, Quaternion.identity) as GameObject;

                    RegisterGameObject.GetComponent<EnemyThrowDirection>().LeftThrow();

                    RegisterGameObject = null;

                    Debug.Log("Spawning Object from Right");

                    break;
            }
           
            // Reset the Spawn Interval Timer
            SpawnInterval = SpawnIntervalRegistry;
        }
        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_Falling : MonoBehaviour
{
    public Transform SpawnPosition;
    [Space]
    public GameObject[] EnemyPrefab;
    [Space]
    public float SpawnArea;
    public int IndexSpawning = 0;
    [Space]
    public float CurrentTime = 0f;
    public float ZeroTime = 0f;

    private float ResetTimeValue;
    private Vector3 RandomizedSpawnArea;

    private void Start()
    {
        ResetTimeValue = CurrentTime;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTime -= Time.deltaTime;

        if(CurrentTime <= ZeroTime)
        {
            CurrentTime = ZeroTime;

            RandomizedSpawnArea = new Vector3(Random.Range(-SpawnArea,SpawnArea), 0f, 0f);

            Instantiate(EnemyPrefab[Random.Range(0,EnemyPrefab.Length)], SpawnPosition.position + RandomizedSpawnArea, SpawnPosition.rotation);

            CurrentTime = ResetTimeValue;

        }
    }
}

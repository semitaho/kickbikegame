using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : IActivator
{

    [SerializeField] GameObject spawnPrefab;

    [SerializeField] Transform playerPrefab;


    [SerializeField] private float spawnDistanceFromPlayer = 10f;

    [SerializeField] private float spawnIntervalInSeconds = 1.5f;

    [SerializeField] private int maxSpawned = 10;

    private int spawnCounter = 0;


    private GameObject player;

    private float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Vector3.Distance(player.transform.position, transform.position) <= spawnDistanceFromPlayer && timer >= spawnIntervalInSeconds)
        {
            Spawn();
            timer = 0;

        }

    }

    public void SpawnFromChaChaEvent()
    {
        if (spawnCounter < maxSpawned)
        {
        Spawn();
        }
    }

    public void TriggerTuuletus() 
    {
        
    }


    void Spawn()
    {
        spawnCounter++;
        Instantiate(spawnPrefab, transform.position, transform.rotation);
    }

    public void ChaChaSpawn()
    {
        
        Instantiate(spawnPrefab,
       new Vector3(playerPrefab.position.x, playerPrefab.position.y + 2, playerPrefab.position.z),
        Quaternion.identity);
    }
}

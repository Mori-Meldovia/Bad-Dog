using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject[] foodPrefabs;

    [SerializeField] private float spawnHeight = 0.1f;

    [SerializeField] private Terrain terrain;
    
    private Vector3 _terrainPosition;
    private Vector3 _terrainSize;
    // Start is called before the first frame update
    void Start()
    {
        _terrainPosition = terrain.GetPosition();
        _terrainSize = terrain.terrainData.size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn a random food at a given location
    public void SpawnFood()
    {
        Vector3 spawnPoint = GetRandomPosition();
        
        // Pick a random food to spawn
        int randomIndex = Random.Range(0,  foodPrefabs.Length);
        GameObject randomFood = foodPrefabs[randomIndex];
        
        // Makes the food hover a bit off the ground to avoid clipping
        Vector3 hoverSpawn = new Vector3(spawnPoint.x, spawnPoint.y + spawnHeight, spawnPoint.z);
        Instantiate(randomFood, hoverSpawn, Quaternion.identity);
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(_terrainPosition.x, _terrainPosition.x + _terrainSize.x);
        float randomZ = Random.Range(_terrainPosition.z, _terrainPosition.z + _terrainSize.z);
        float randomY = terrain.SampleHeight(new Vector3(randomX, 0f, randomZ)) + _terrainPosition.y;
        return new Vector3(randomX, randomY, randomZ);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject[] foodPrefabs;

    [SerializeField] private float spawnHeight = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawn a random food at a given location
    public void SpawnFood(Transform spawnPoint)
    {
        // Pick a random food to spawn
        int randomIndex = Random.Range(0,  foodPrefabs.Length);
        GameObject randomFood = foodPrefabs[randomIndex];
        // Makes the food hover a bit off the ground to avoid clipping
        Vector3 hoverSpawn = new Vector3(spawnPoint.position.x, spawnPoint.position.y + spawnHeight, spawnPoint.position.z);
        Instantiate(randomFood, hoverSpawn, spawnPoint.rotation);
    }
}

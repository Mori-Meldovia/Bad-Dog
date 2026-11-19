using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Food Spawning")]
    [SerializeField] private float foodSpawnInterval = 3f;
    [SerializeField] private FoodManager foodManager;
    
    private bool _foodSpawning = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnFood());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnFood()
    {
        while (_foodSpawning)
        {
            foodManager.SpawnFood();
            yield return new WaitForSeconds(foodSpawnInterval);
        }
    }
}

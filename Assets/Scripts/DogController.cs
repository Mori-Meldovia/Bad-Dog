using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    [SerializeField] private float eatingDistance = 0.5f;
    [SerializeField] private float eatingInterval = 0.3f;
    [SerializeField] private float biteSize = 3.5f;

    private FoodController _foodController;
    private Coroutine _eatingRoutine;
    // Start is called before the first frame update
    void Start()
    {
        _foodController = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DetectFood()
    {
        if (Input.GetButtonDown("Eat"))
        {
            Debug.Log("Eating detected");
            // Detect closest food to eat
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, eatingDistance))
            {
                _foodController = hit.collider.gameObject.GetComponent<FoodController>();
                _eatingRoutine = StartCoroutine(BiteFood());
            }
        } else if (Input.GetButtonUp("Eat"))
        {
            // Clear current food data and stop eating
            _foodController = null;
            StopCoroutine(_eatingRoutine);
        }
    }

    IEnumerator BiteFood()
    {
        // While eating "bite" the food every some time interval
        while (Input.GetButton("Eat"))
        {
            _foodController.eatFood(biteSize);
            yield return new WaitForSeconds(eatingInterval);
        }
    }
}

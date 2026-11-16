using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodController : MonoBehaviour
{

    [Header("Poop Meter")]
    [SerializeField] private int poopValue;
    [SerializeField] private GameObject poopMeter;
    [SerializeField] private float foodAmount;
    
    [Header("Food Hover")]
    [SerializeField]
    private float hoverAmplitude = 0.03f;
    [SerializeField]
    private float hoverSpeed = 1.5f;
    [SerializeField]
    private float rotateSpeed = 0.04f;
    
    private PoopMeterController _poopController;
    private float _startTime;
    private bool _eating;
    
    // Start is called before the first frame update
    void Start()
    {
       _poopController = poopMeter.GetComponent<PoopMeterController>();
       _startTime = Time.time;
       _eating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (foodAmount <= 0)
        {
            _poopController.incrementMeter(poopValue);
            Destroy(this.gameObject);
        }
        if (!_eating)
        {
            // Makes food hover and rotate
            transform.position = new Vector3(transform.position.x, hoverAmplitude * Mathf.Sin(hoverSpeed * (Time.time - _startTime)), transform.position.z);
            transform.Rotate(new Vector3(0f, rotateSpeed, 0f));
        }
    }

    // Set to true if the dog is eating food
    public void setEating(bool isEating)
    {
        _eating = isEating;
    }

    public void eatFood(float amount)
    {
        if (amount > 0)
        {
            foodAmount -= amount;
        }
    }
}

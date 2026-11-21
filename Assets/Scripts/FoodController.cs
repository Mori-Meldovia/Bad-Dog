using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodController : MonoBehaviour
{

    [Header("Poop Meter")]
    [SerializeField] private int poopValue;
    [SerializeField] private float foodAmount;
    
    [Header("Food Hover")]
    [SerializeField]
    private float hoverAmplitude = 0.02f;
    [SerializeField]
    private float hoverSpeed = 1.5f;
    [SerializeField]
    private float rotateSpeed = 0.04f;
    
    private PoopMeterController _poopController;
    private float _startTime;
    private bool _eating;
    private float _initial_y;
    
    // Start is called before the first frame update
    void Start()
    {
       _poopController = GameObject.FindWithTag("PoopMeter").GetComponent<PoopMeterController>();
       _startTime = Time.time;
       _eating = false;
       _initial_y = transform.position.y;
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
            transform.position = new Vector3(transform.position.x, _initial_y + hoverAmplitude * Mathf.Sin(hoverSpeed * (Time.time - _startTime)), transform.position.z);
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

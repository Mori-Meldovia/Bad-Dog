
using UnityEngine;

public class FoodController : MonoBehaviour
{
    [Header("Food Settings")]
    [SerializeField] private float foodAmount = 10f;

    [Header("Food Hover")]
    [SerializeField] private float hoverAmplitude = 0.02f;
    [SerializeField] private float hoverSpeed = 1.5f;
    [SerializeField] private float rotateSpeed = 0.04f;

    private PoopMeterController poopController;

    private float startTime;
    private float initialY;

    private bool eating;
    private bool foodFinished;

    private void Start()
    {
        poopController = FindObjectOfType<PoopMeterController>();

        if (poopController == null)
        {
            Debug.LogWarning(
                "Food could not find the poop meter!"
            );
        }

        startTime = Time.time;
        eating = false;
        initialY = transform.position.y;
    }

    private void Update()
    {
        // Destroy the food after all of it has been eaten.
        if (foodAmount <= 0f && !foodFinished)
        {
            foodFinished = true;

            Debug.Log("Food completely eaten!");

            Destroy(gameObject);
            return;
        }

        // Hover and rotate while the dog is not eating.
        if (!eating)
        {
            float newY = initialY +
                Mathf.Sin((Time.time - startTime) * hoverSpeed)
                * hoverAmplitude;

            transform.position = new Vector3(
                transform.position.x,
                newY,
                transform.position.z
            );

            transform.Rotate(
                Vector3.up,
                rotateSpeed
            );
        }
    }

    public void setEating(bool isEating)
    {
        eating = isEating;
    }

    public void eatFood(float amount)
    {
        if (amount <= 0f || foodAmount <= 0f)
        {
            return;
        }

        // Only count the amount of food that actually remains.
        float amountEaten = Mathf.Min(amount, foodAmount);

        foodAmount -= amountEaten;

        // Add the actual food eaten to the poop meter.
        if (poopController != null)
        {
            poopController.incrementMeter(amountEaten);
        }

        Debug.Log("Food eaten: " + amountEaten);
        Debug.Log("Food remaining: " + foodAmount);
    }
}

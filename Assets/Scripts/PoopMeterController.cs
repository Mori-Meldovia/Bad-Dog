using UnityEngine;

public class PoopMeterController : MonoBehaviour
{
    public float poopMeterValue = 0f;

    public void incrementMeter(float amount)
    {
        poopMeterValue += amount;

        Debug.Log("Poop meter: " + poopMeterValue);
    }

    public void decrementMeter(float amount)
    {
        poopMeterValue -= amount;

        if (poopMeterValue < 0f)
        {
            poopMeterValue = 0f;
        }

        Debug.Log("Poop meter: " + poopMeterValue);
    }
}

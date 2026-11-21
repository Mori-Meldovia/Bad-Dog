using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopMeterController : MonoBehaviour
{
    public int poopMeterValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void incrementMeter(int amount)
    {
        poopMeterValue += amount;
    }

    public void decrementMeter(int amount)
    {
        poopMeterValue -= amount;
    }
}

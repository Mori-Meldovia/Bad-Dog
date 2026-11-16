using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodController : MonoBehaviour
{

    [Header("Poop Meter")]
    [SerializeField] private int poopValue;
    
    [Header("Food Hover")]
    [SerializeField]
    private float hoverAmplitude = 0.03f;
    [SerializeField]
    private float hoverSpeed = 1.5f;

    [SerializeField] private float rotateSpeed = 0.04f;

    private Animator animator;
    private int foodStage = 3;

    private float startTime;
    // Start is called before the first frame update
    void Start()
    { 
        // for future implementation of eating animations
       // animator = GetComponent<Animator>(); 
       startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Makes food hover up and down
        transform.position = new Vector3(transform.position.x, hoverAmplitude * Mathf.Sin(hoverSpeed * (Time.time - startTime)), transform.position.z);
        
        transform.Rotate(new Vector3(0f, rotateSpeed, 0f));
    }

    public void biteFood()
    {
        animator.SetInteger("FoodStage", foodStage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class OwnerController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Insert Character Controller")]
    private CharacterController controller;

    [SerializeField]
    [Tooltip("Insert Animator Controller")]
    private Animator ownerAnimator;

    [SerializeField]
    [Tooltip("Insert Tracked Object (Player)")]
    private Transform tracked;

    [SerializeField]
    private float currSpeed = 2.0f;

    private float startSpeed = 2.0f;

    private float maxSpeed = 10.0f;

    private float timeInterval = 15.0f;
    private float timeClock = 0.0f;

    private float speedIncrement = 1.0f;

    private float speedToRun = 4.0f;

    private float swipeDistance = 2.0f;

    private bool isOnPoop;

    // Start is called before the first frame update
    void Start()
    {
        currSpeed = startSpeed;
        timeClock = 0.0f;
        isOnPoop = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeClock += Time.deltaTime;
        // VERY BASIC TEST IMPLEMENTATION

        Transform playerTransform = transform;
        // Ground Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = (playerTransform.right * x) + (playerTransform.forward * z);
        controller.Move(movement * Time.deltaTime * currSpeed);

        UpdateAnimator(movement);
        UpdateSpeed();
    }

    public IEnumerable activatePoopAnimation()
    {
        ownerAnimator.SetBool("isOnPoop", true);
        isOnPoop = true;
        // Wait one frame for the Animator to enter the new state
        yield return null;

        float duration = ownerAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duration);

        ownerAnimator.SetBool("isOnPoop", false);
        isOnPoop = false;

        // needs method to know when its not on poop
    }

    private void UpdateAnimator(Vector3 movement)
    {
        print(getPlayerProximity());
        // Priority of Animations: Poop (Not in this animator), Swipe, Movement: run, walk
        if (!isOnPoop && getPlayerProximity() <= swipeDistance)
        {
            ownerAnimator.SetBool("isNearPlayer", true);
            ownerAnimator.SetBool("isMoving", false);
        }

        else if (!isOnPoop && movement.magnitude > 0 && currSpeed > speedToRun)
        {
            ownerAnimator.SetBool("isNearPlayer", false);
            ownerAnimator.SetBool("isMoving", true);
            ownerAnimator.SetBool("isFast", true);
        }

        else if (!isOnPoop && movement.magnitude > 0 && currSpeed > 0)
        {
            ownerAnimator.SetBool("isNearPlayer", false);
            ownerAnimator.SetBool("isMoving", true);
            ownerAnimator.SetBool("isFast", false);
        }

        else
        {
            ownerAnimator.SetBool("isNearPlayer", false);
            ownerAnimator.SetBool("isMoving", false);
        }
    }

    private void UpdateSpeed()
    {
        if (timeClock >= timeInterval && currSpeed <= maxSpeed)
        {
            timeClock = 0;
            currSpeed += speedIncrement;
        }
    }


    // returns player's speed!
    private float getPlayerProximity()
    {
        return Vector3.Distance(controller.transform.position, tracked.position);
    }
}


using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DogController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float turnSpeed = 360f;
    [SerializeField] private float acceleration = 12f;

    [Header("Eating")]
    [SerializeField] private float eatingDistance = 1.5f;
    [SerializeField] private float eatingInterval = 0.3f;
    [SerializeField] private float biteSize = 3.5f;

    [Header("Pooping")]
    [SerializeField] private GameObject normalPoopPrefab;
    [SerializeField] private GameObject goldenPoopPrefab;
    [SerializeField] private Transform poopSpawnPoint;
    [SerializeField] private float poopCost = 5f;
    [SerializeField, Range(0f, 1f)]
    private float goldenPoopChance = 0.1f;

    // Adjust this if the poop model is still sideways.
    [SerializeField] private Vector3 poopRotationOffset =
        new Vector3(90f, 0f, 0f);

    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private PoopMeterController poopMeter;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip eatSound;
    [SerializeField] private AudioClip poopSound;

    // Animator parameters.
    private static readonly int IsWalking =
        Animator.StringToHash("IsWalking");

    private static readonly int IsRunning =
        Animator.StringToHash("IsRunning");

    private static readonly int IsEating =
        Animator.StringToHash("IsEating");

    private static readonly int IsJumping =
        Animator.StringToHash("IsJumping");

    private static readonly int Poop =
        Animator.StringToHash("Poop");

    // Eating variables.
    private FoodController foodController;
    private Coroutine eatingRoutine;

    // Movement variables.
    private float verticalVelocity;
    private float currentSpeed;

    private void Start()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (poopMeter == null)
        {
            poopMeter = FindObjectOfType<PoopMeterController>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (characterController == null)
        {
            Debug.LogError("CharacterController is missing!");
        }

        if (animator == null)
        {
            Debug.LogWarning("Dog Animator is missing!");
        }

        if (poopMeter == null)
        {
            Debug.LogWarning("Poop meter is missing!");
        }
    }

    private void Update()
    {
        HandleMovement();
        DetectFood();
        HandlePooping();
    }

    private void HandleMovement()
    {
        if (characterController == null)
        {
            return;
        }

        bool grounded = characterController.isGrounded;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Turn the dog left and right.
        transform.Rotate(
            Vector3.up,
            horizontal * turnSpeed * Time.deltaTime
        );

        // Move forward and backward.
        Vector3 movement = transform.forward * vertical;

        bool isMoving = Mathf.Abs(vertical) > 0.1f;

        // Run only while moving and holding Left Shift.
        bool running =
            isMoving && Input.GetKey(KeyCode.LeftShift);

        float targetSpeed = 0f;

        if (isMoving)
        {
            targetSpeed = running ? runSpeed : walkSpeed;
        }

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            acceleration * Time.deltaTime
        );

        if (grounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        // Jump with Space.
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );

            PlaySound(jumpSound);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = movement * currentSpeed;
        velocity.y = verticalVelocity;

        characterController.Move(
            velocity * Time.deltaTime
        );

        // Update animations.
        if (animator != null)
        {
            animator.SetBool(
                IsWalking,
                isMoving && !running
            );

            animator.SetBool(
                IsRunning,
                running
            );

            animator.SetBool(
                IsJumping,
                !characterController.isGrounded
            );
        }
    }

    private void DetectFood()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (eatingRoutine == null)
            {
                Vector3 detectionCenter =
                    transform.position + Vector3.up * 0.5f;

                Collider[] hits = Physics.OverlapSphere(
                    detectionCenter,
                    eatingDistance,
                    Physics.DefaultRaycastLayers,
                    QueryTriggerInteraction.Collide
                );

                Debug.Log(
                    "Food detection found " + hits.Length +
                    " colliders."
                );

                foreach (Collider hit in hits)
                {
                    FoodController food =
                        hit.GetComponentInParent<FoodController>();

                    if (food != null)
                    {
                        Debug.Log("FOOD FOUND: " + hit.name);

                        foodController = food;
                        foodController.setEating(true);

                        eatingRoutine =
                            StartCoroutine(BiteFood());

                        break;
                    }
                }

                if (eatingRoutine == null)
                {
                    Debug.Log("No food detected nearby!");
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            StopEating();
        }
    }

    private IEnumerator BiteFood()
    {
        if (animator != null)
        {
            animator.SetBool(IsEating, true);
        }

        // Keep eating while E is held and the food still exists.
        while (Input.GetKey(KeyCode.E) &&
               foodController != null)
        {
            foodController.eatFood(biteSize);

            PlaySound(eatSound);

            yield return new WaitForSeconds(eatingInterval);
        }

        if (foodController != null)
        {
            foodController.setEating(false);
        }

        foodController = null;
        eatingRoutine = null;

        if (animator != null)
        {
            animator.SetBool(IsEating, false);
        }
    }

    private void StopEating()
    {
        if (eatingRoutine != null)
        {
            StopCoroutine(eatingRoutine);
            eatingRoutine = null;
        }

        if (foodController != null)
        {
            foodController.setEating(false);
            foodController = null;
        }

        if (animator != null)
        {
            animator.SetBool(IsEating, false);
        }
    }

    private void HandlePooping()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryPoop();
        }
    }

    private void TryPoop()
    {
        if (poopMeter == null)
        {
            Debug.LogWarning("Poop meter is missing!");
            return;
        }

        if (normalPoopPrefab == null)
        {
            Debug.LogWarning("Normal poop prefab is missing!");
            return;
        }

        // Check whether enough food has been eaten.
        if (poopMeter.poopMeterValue < poopCost)
        {
            Debug.Log(
                "Not enough food eaten to poop! Current meter: " +
                poopMeter.poopMeterValue
            );
            return;
        }

        // Randomly choose golden or normal poop.
        bool isGolden =
            goldenPoopPrefab != null &&
            Random.value < goldenPoopChance;

        GameObject poopPrefab = isGolden
            ? goldenPoopPrefab
            : normalPoopPrefab;

        // Determine the spawn position.
        Vector3 spawnPosition;

        if (poopSpawnPoint != null)
        {
            spawnPosition = poopSpawnPoint.position + Vector3.up * 0.05f;
        }
        else
        {
            spawnPosition =
                transform.position - transform.forward * 0.7f;
        }

        // Apply the rotation correction to the poop model.
        Quaternion spawnRotation =
            Quaternion.Euler(poopRotationOffset);

        Instantiate(
            poopPrefab,
            spawnPosition,
            spawnRotation
        );

        // Spend 5 food points for each poop.
        poopMeter.decrementMeter(poopCost);

        if (animator != null)
        {
            animator.SetTrigger(Poop);
        }

        PlaySound(poopSound);

        Debug.Log(
            isGolden
                ? "Golden poop spawned!"
                : "Normal poop spawned!"
        );

        Debug.Log(
            "Remaining food points: " +
            poopMeter.poopMeterValue
        );
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void OnDisable()
    {
        StopEating();
    }
}

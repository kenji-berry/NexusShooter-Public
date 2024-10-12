using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private GameController gameController;
    public Camera playerCamera;

    [Header("Movement")]
    public float speed = 6f;
    public float sprintSpeedMultiplier = 1.4f;
    public float mouseSensitivity = 0.1f;
    public float gravity = 14f;
    public float jumpForce = 8f;
    public float playerFOV = 90f;
    public float verticalRotation = 0f;
    public float standHeight = 2f;
    public float currentHeight = 2f;
    public float crouchHeight = 1f;
    public Vector2 moveValue, lookValue;
    public Vector3 characterVelocity;
    public float verticalVelocity;

    public bool isSprinting = false;
    public bool isJumping = false;
    public bool isGrounded = true;
    public bool isCrouched = false;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        currentHealth = maxHealth;
        gameController = FindFirstObjectByType<GameController>();

        // Initialize the health bar with the player's starting health
        gameController.UpdateHealthBar(currentHealth, maxHealth);
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        lookValue = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    void OnJump(InputValue value)
    {
        if (!controller.isGrounded) return;
        isJumping = value.isPressed;
    }

    void OnCrouch(InputValue value)
    {
        isCrouched = value.isPressed;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, (lookValue.x * mouseSensitivity), 0.0f));
        verticalRotation += (-lookValue.y * mouseSensitivity);
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.transform.localEulerAngles = new Vector3(verticalRotation, 0.0f, 0.0f);

        handleSprintFOV();
        handleCrouch();

        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -1f;
            } 

            if (isJumping)
            {
                verticalVelocity = jumpForce;
                isJumping = false;
            }
        } else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 moveVector = new Vector3(moveValue.x, 0f, moveValue.y);
        Vector3 targetVelocity = transform.TransformVector(moveVector) * speed;

        if (isSprinting) targetVelocity *= sprintSpeedMultiplier;

        characterVelocity = new Vector3(targetVelocity.x, verticalVelocity, targetVelocity.z);

        controller.Move(characterVelocity * Time.deltaTime);
    }

    void handleSprintFOV()
    {
        if (isSprinting)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, playerFOV + 10f, 10f * Time.deltaTime);
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, playerFOV, 10f * Time.deltaTime);
        }
    }

    void handleCrouch()
    {
        if (isCrouched)
        {
            currentHeight = Mathf.Lerp(currentHeight, crouchHeight, 10f * Time.deltaTime);
            controller.height = controller.GetComponent<CapsuleCollider>().height = currentHeight;
        } else
        {
            currentHeight = Mathf.Lerp(currentHeight, standHeight, 10f * Time.deltaTime);
            controller.height = controller.GetComponent<CapsuleCollider>().height = currentHeight;
        }
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        gameController.UpdateHealthBar(currentHealth, maxHealth);

    }

    // Method to heal
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        gameController.UpdateHealthBar(currentHealth, maxHealth);
        // UpdateHealthBar();
    }
}

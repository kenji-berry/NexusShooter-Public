using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    public Camera playerCamera;

    [Header("Movement")]
    public float speed = 10f;
    public float sprintSpeedMultiplier = 1.5f;
    public float mouseSensitivity = 0.1f;
    public float gravitationalForce = 30f;
    public float jumpForce = 10f;
    public float playerFOV = 60f;
    public float verticalRotation = 0f;
    public float standHeight = 2f;
    public float currentHeight = 2f;
    public float crouchHeight = 1f;
    public bool isSprinting = false;
    public bool isJumping = false;
    public bool isGrounded = true;
    public bool isCrouched = false;
    public Vector2 moveValue, lookValue;
    public Vector3 characterVelocity;
    public float verticalVelocity;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    private GameController gameController;

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
            if (isJumping)
            {
                verticalVelocity = jumpForce;
                isJumping = false;
            }
        } else
        {
            verticalVelocity -= gravitationalForce * Time.deltaTime;

        }

        Vector3 moveVector = new Vector3(moveValue.x, 0f, moveValue.y);
        Vector3 targetVelocity = transform.TransformVector(moveVector) * speed;

        if (isSprinting) targetVelocity *= sprintSpeedMultiplier;

        characterVelocity.x = Mathf.Lerp(characterVelocity.x, targetVelocity.x, 10f * Time.deltaTime);
        characterVelocity.y = verticalVelocity;
        characterVelocity.z = Mathf.Lerp(characterVelocity.z, targetVelocity.z, 10f * Time.deltaTime);

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

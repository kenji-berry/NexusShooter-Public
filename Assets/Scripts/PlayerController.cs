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
    public float gravitationalForce = 100f;
    public float jumpForce = 30f;
    public float playerFOV = 60f;
    public float verticalRotation = 0f;
    public bool isSprinting = false;
    public bool isJumping = false;
    public bool isGrounded = true;
    public Vector2 moveValue, lookValue;
    public Vector3 CharacterVelocity;

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
        isJumping = value.isPressed;
    }

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, (lookValue.x * mouseSensitivity), 0.0f));
        verticalRotation += (-lookValue.y * mouseSensitivity);
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.transform.localEulerAngles = new Vector3(verticalRotation, 0.0f, 0.0f);

        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        Vector3 targetVelocity = transform.TransformVector(movement) * speed;

        if (isSprinting)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, playerFOV + 10f, 10f * Time.deltaTime);
            targetVelocity *= sprintSpeedMultiplier;
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, playerFOV, 10f * Time.deltaTime);
        }

        CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity, 10f * Time.deltaTime);

        isGrounded = controller.isGrounded;
        if (isGrounded && isJumping)
        {
            CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);
            CharacterVelocity += Vector3.up * jumpForce;
            isJumping = false;
        } 

        CharacterVelocity += Vector3.down * gravitationalForce * Time.deltaTime;

        controller.Move(CharacterVelocity * Time.deltaTime);
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
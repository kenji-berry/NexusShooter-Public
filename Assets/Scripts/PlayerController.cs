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
    public float crouchSpeedMultiplier = 0.5f;

    public float gravity = 18f;
    public float friction = 6f;
    public float jumpForce = 8f;

    public float mouseSensitivity = 0.1f;
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
        handleLook();

        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -1f;
            } 

            if (isJumping)
            {
                verticalVelocity += jumpForce;
                isJumping = false;
            }
        } else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 moveVector = new Vector3(moveValue.x, 0f, moveValue.y);
        Vector3 targetVelocity = transform.TransformVector(moveVector);

        Accelerate(targetVelocity, speed, 10f);
        ApplyFriction();

        characterVelocity.y = verticalVelocity;

        handleSprint();
        handleCrouch();

        controller.Move(characterVelocity * Time.deltaTime);
    }

    void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float addSpeed, accelSpeed, currSpeed;
        float wishSpeed = wishspeed;

        if (!isGrounded) wishSpeed = Mathf.Min(wishSpeed, 20f);
        if (isSprinting) wishSpeed *= sprintSpeedMultiplier;
        if (isCrouched) wishSpeed *= crouchSpeedMultiplier;

        currSpeed = Vector3.Dot(characterVelocity, wishdir);

        addSpeed = wishSpeed - currSpeed;
        if (addSpeed <= 0) return;

        accelSpeed = accel * Time.deltaTime * wishSpeed;
        if (accelSpeed > addSpeed) accelSpeed = addSpeed;

        characterVelocity.x += accelSpeed * wishdir.x;
        characterVelocity.z += accelSpeed * wishdir.z;
    }

    void ApplyFriction()
    {
        Vector3 currVelocity = new Vector3(characterVelocity.x, 0f, characterVelocity.z);

        float currSpeed = currVelocity.magnitude; // calculates speed in xz plane
        float drop = 0f;
        float controlSpeed;

        if (isGrounded)
        {
            currVelocity.y = characterVelocity.y;
            controlSpeed = currSpeed < 10f ? 10f : currSpeed;
            drop += controlSpeed * 3f * Time.deltaTime;
        }

        float newSpeed = Mathf.Max(currSpeed - drop, 0f); // calculates new speed after decreasing drop

        if (currSpeed > 0f) newSpeed /= currSpeed;

        characterVelocity *= newSpeed;
    }

    void handleLook()
    {
        lookValue *= mouseSensitivity;
        transform.Rotate(new Vector3(0.0f, (lookValue.x), 0.0f));

        verticalRotation -= lookValue.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    void handleSprint()
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

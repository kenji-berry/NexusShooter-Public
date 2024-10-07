using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveValue;
    public Vector3 CharacterVelocity;
    public float speed = 10f;
    private CharacterController controller;
  

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    void Update()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);
        Vector3 targetVelocity = transform.TransformVector(movement) * speed;
        CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity, 10f * Time.deltaTime);
        controller.Move(CharacterVelocity * Time.deltaTime);
    }
}
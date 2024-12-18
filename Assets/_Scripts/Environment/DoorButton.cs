using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorButton : MonoBehaviour
{
    public SoundController soundController;

    public GameObject door;
    public Camera playerCamera;

    private bool isPressed = false;

    public Material pressedColor;

    void Awake()
    {
        soundController = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundController>();
    }

    void OnUse(InputValue value)
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, 2f))
        {
            if (!isPressed && hit.collider.gameObject == this.gameObject)
            {
                soundController.Play(soundController.doorButtonPress);

                door.GetComponent<LockedDoubleSlidingDoor>().Open();

                isPressed = true;

                ChangeAppearance();
            }
        }
    }

    void ChangeAppearance()
    {
        this.GetComponent<Renderer>().material = pressedColor;
    }
}

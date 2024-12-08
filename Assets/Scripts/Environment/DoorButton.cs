using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorButton : MonoBehaviour
{
    public GameObject door;
    public Camera playerCamera;

    void OnUse(InputValue value)
    {
        Debug.Log("using");
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, 2f))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                Debug.Log("opening door");
                door.GetComponent<SlidingDoor>().Open();
            }
        }
    }
}

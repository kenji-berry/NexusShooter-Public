using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public AutomaticSlidingDoor door;

    void OnTriggerEnter(Collider collider)
    {
        CharacterController controller = collider.transform.GetComponent<CharacterController>();

        if (controller != null)
        {
            if (!door.isOpen) door.Open();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        CharacterController controller = collider.transform.GetComponent<CharacterController>();

        if (controller)
        {
            if (door.isOpen) door.Close();
        }
    }
}
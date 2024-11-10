using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameController gameController;

    void OnTriggerEnter(Collider collider)
    {
        CharacterController controller = collider.transform.GetComponent<CharacterController>();

        if (controller != null)
        {
            gameController.CompleteLevel();
        }
    }
}

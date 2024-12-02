using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AutomaticSlidingDoor : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;

    public bool isOpen = false;

    public float speed = 1f;
    public float slideAmount = 1.9f;

    private Coroutine AnimationCoroutine;

    private Vector3 leftDoorStartPos;
    private Vector3 rightDoorStartPos;

    public Vector3 SlideDirection = Vector3.left;

    private void Awake()
    {
        leftDoorStartPos = leftDoor.transform.position;
        rightDoorStartPos = rightDoor.transform.position;
    }

    public void Open()
    {
        if (!isOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(OpenSlidingDoor());
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(CloseSlidingDoor());
        }
    }

    private IEnumerator OpenSlidingDoor()
    {
        Vector3 leftDoorEndPos = leftDoorStartPos + slideAmount * SlideDirection;
        Vector3 rightDoorEndPos = rightDoorStartPos + slideAmount * (SlideDirection * -1);

        Vector3 leftDoorStart = leftDoor.transform.position;
        Vector3 rightDoorStart = rightDoor.transform.position;

        float time = 0;
        isOpen = true;

        while (time < 1)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoorStart, leftDoorEndPos, time);
            rightDoor.transform.position = Vector3.Lerp(rightDoorStart, rightDoorEndPos, time);

            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    private IEnumerator CloseSlidingDoor()
    {
        Vector3 leftDoorEndPos = leftDoorStartPos;
        Vector3 rightDoorEndPos = rightDoorStartPos;

        Vector3 leftDoorStart = leftDoor.transform.position;
        Vector3 rightDoorStart = rightDoor.transform.position;

        float time = 0;
        isOpen = false;

        while (time < 1)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoorStart, leftDoorEndPos, time); ;
            rightDoor.transform.position = Vector3.Lerp(rightDoorStart, rightDoorEndPos, time);

            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}

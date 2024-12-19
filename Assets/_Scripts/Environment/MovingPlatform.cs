using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool hasMoved = false;

    public Vector3 moveDirection = Vector3.down;
    public float moveAmount = 11f;
    public float speed = 1f;

    private Coroutine AnimationCoroutine;
    private Vector3 startPos;

    void Awake()
    {
        startPos = transform.position;
    }

    public void Move()
    {
        if (!hasMoved)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(MovePlatform());
        }
    }

    private IEnumerator MovePlatform()
    {
        Vector3 endPos = startPos + moveAmount * moveDirection;
        Vector3 start = transform.position;

        float time = 0;
        hasMoved = true;

        while (time < 1)
        {
            transform.position = Vector3.Lerp(start, endPos, time);

            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}

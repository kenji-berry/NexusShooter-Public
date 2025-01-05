using UnityEngine;
using System.Collections;

public class PlatformMover : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Movement Settings")]
    public float speed = 2f;
    public bool pingPong = true;
    public float slowdownDistance = 1f;
    public float pauseDuration = 0.2f;

    private Vector3 targetPosition;
    private bool movingToB = true;
    private bool isPaused = false;

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            enabled = false;
            return;
        }
        transform.position = pointA.position;
        targetPosition = pointB.position;
    }

    void Update()
    {
        if (!isPaused)
        {
            MovePlatform();
        }
    }

    void MovePlatform()
    {
        Vector3 startingPoint = movingToB ? pointA.position : pointB.position;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        float distanceFromStart = Vector3.Distance(transform.position, startingPoint);

        float currentSpeed = speed;

        if (distanceToTarget < slowdownDistance)
        {
            float slowdownFactor = distanceToTarget / slowdownDistance;
            currentSpeed *= slowdownFactor;
            currentSpeed = Mathf.Max(currentSpeed, speed * 0.1f);
        }

        if (distanceFromStart < slowdownDistance)
        {
            float accelerationFactor = distanceFromStart / slowdownDistance;
            currentSpeed *= accelerationFactor;
            currentSpeed = Mathf.Max(currentSpeed, speed * 0.1f);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

        if (distanceToTarget < 0.01f)
        {
            StartCoroutine(PauseAtPoint());
            if (pingPong)
            {
                movingToB = !movingToB;
                targetPosition = movingToB ? pointB.position : pointA.position;
            }
            else
            {
                enabled = false;
            }
        }
    }

    IEnumerator PauseAtPoint()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        isPaused = false;
    }

    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointB.position, 0.2f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}

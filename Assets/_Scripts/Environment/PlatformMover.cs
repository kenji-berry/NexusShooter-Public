using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform pointA; // Starting point
    public Transform pointB; // Ending point

    [Header("Movement Settings")]
    public float speed = 2f;          // Speed of movement
    public bool pingPong = true;      // If true, platform moves back and forth

    private Vector3 targetPosition;
    private bool movingToB = true;

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Point A and Point B must be assigned in the inspector.");
            enabled = false;
            return;
        }

        // Initialize the platform's position to Point A
        transform.position = pointA.position;
        targetPosition = pointB.position;
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the platform has reached the target
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            if (pingPong)
            {
                // Switch target between Point A and Point B
                movingToB = !movingToB;
                targetPosition = movingToB ? pointB.position : pointA.position;
            }
            else
            {
                // If not ping-pong, stop moving once it reaches Point B
                enabled = false;
            }
        }
    }

    // Optional: Visualize the points in the editor
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

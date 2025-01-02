using UnityEngine;

public class ItemBounce : MonoBehaviour
{
    public float bounceHeight = 0.1f;
    public float bounceSpeed = 3f;
    public float rotationSpeed = 40f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        float newY = originalPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }
}
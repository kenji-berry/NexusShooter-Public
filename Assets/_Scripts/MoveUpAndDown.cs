using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    public float minY = -2000f;
    public float maxY = 0f;   
    public float speed = 1f;   

    void Update()
    {
        // Move the object between minY and maxY
        float newY = Mathf.PingPong(Time.time * speed, maxY - minY) + minY;
        
        // Apply the new Y position to the object
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

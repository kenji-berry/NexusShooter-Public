using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCanvasOrbit : MonoBehaviour
{
    public Transform target;         // The object to orbit around
    public Camera mainCamera;
    public Canvas uiCanvas;  
    public float orbitSpeed = 20f;   
    public float distance = 10f; 
    public Vector3 axis = Vector3.up; // Axis to orbit around (default: Y-axis)

    private Vector3 initialCameraPosition;
    private Vector3 initialCanvasPosition;
    private float currentAngle = 0f;

    void Start()
    {
        if (mainCamera != null && target != null)
        {
            initialCameraPosition = mainCamera.transform.position;
            initialCanvasPosition = uiCanvas.transform.position;
        }
    }

    void Update()
    {
        if (target != null && mainCamera != null && uiCanvas != null)
        {
            // Continuous rotation based on time
            currentAngle += orbitSpeed * Time.deltaTime;
            
            // Calculate new position
            float x = target.position.x + Mathf.Cos(currentAngle * Mathf.Deg2Rad) * distance;
            float z = target.position.z + Mathf.Sin(currentAngle * Mathf.Deg2Rad) * distance;
            Vector3 newPosition = new Vector3(x, mainCamera.transform.position.y, z);
            
            // Update camera position
            mainCamera.transform.position = newPosition;
            mainCamera.transform.LookAt(target.position);
            
            // Update canvas to match camera
            uiCanvas.transform.position = mainCamera.transform.position;
            uiCanvas.transform.rotation = mainCamera.transform.rotation;
        }
    }
}

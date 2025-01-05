using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenToBlack : MonoBehaviour
{
    public Camera cam;

    void Start()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;
            cam.cullingMask = 0;
        }
    }
}
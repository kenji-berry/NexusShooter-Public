using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LightColorCycleSwitch : MonoBehaviour
{
    public Light targetLight; 
    public Color[] colors; 
    public float cycleDuration = 5f; 
    private int currentColorIndex = 0;
    private float timer = 0f;

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        if (timer >= cycleDuration)
        {
            // Reset the timer and switch to the next color
            timer = 0f;
            SwitchColor();
        }
    }

    void SwitchColor()
    {
        // Change to the next color in the array
        currentColorIndex = (currentColorIndex + 1) % colors.Length;
        targetLight.color = colors[currentColorIndex];
    }
}

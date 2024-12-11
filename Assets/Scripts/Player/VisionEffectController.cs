using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class VisionEffectController : MonoBehaviour
{
    public PostProcessVolume postProcessVolume; // Reference to the Post Process Volume
    private DepthOfField depthOfField;

    private void Start()
    {
        postProcessVolume.profile.TryGetSettings(out depthOfField);
        StartCoroutine(PulsateFocalLength());
    }

    private IEnumerator PulsateFocalLength()
    {
        float[] focalLengths = { 40f, 25f, 40f, 20f, 35f, 15f, 30f, 15f, 25f, 15f, 25f, 15f,0};
        int index = 0;

        while (true)
        {
            float startValue = depthOfField.focalLength.value;
            float endValue = focalLengths[index];
            float elapsedTime = 0f;
            float duration = 1f;

            // Lerp the focal length value over time to create a pulsating effect
            while (elapsedTime < duration)
            {
                depthOfField.focalLength.value = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            depthOfField.focalLength.value = endValue;
            // Stop the coroutine when the focal length reaches 0
            if (endValue == 0f)
            {
                yield break; 
            }
            index = (index + 1) % focalLengths.Length;
        }
    }
}
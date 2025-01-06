using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject endElement;
    public float delayBeforeFadeIn = 2.0f;
    public float delayBeforeGunshot = 3.0f; // Delay before playing the gunshot sound
    public SoundController soundController; // Reference to the SoundController

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered end game trigger");
            StartCoroutine(DelayedFadeIn());
        }
    }

    private IEnumerator DelayedFadeIn()
    {
        yield return new WaitForSeconds(delayBeforeGunshot);
        PlayGunshotSound();
        yield return new WaitForSeconds(delayBeforeFadeIn);
        StartCoroutine(FadeInEndElement());
    }

    private IEnumerator FadeInEndElement()
    {
        Debug.Log("Fading in end element");
        endElement.SetActive(true);
        CanvasGroup canvasGroup = endElement.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = endElement.AddComponent<CanvasGroup>();
        }

        float duration = 1.0f; // Duration of the fade-in effect
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private void PlayGunshotSound()
    {
        Debug.Log("Playing gunshot sound");
        if (soundController != null)
        {
            soundController.Play(soundController.finalShot);
        }
    }
}
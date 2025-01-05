using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenToBlack : MonoBehaviour
{
    [SerializeField] private Image blackOverlay;
    [SerializeField] private float fadeSpeed = 1f;
    private Canvas[] allCanvases;
    private bool hasFaded = false;

    private void Start()
    {
        // Cache all canvases in the scene
        allCanvases = FindObjectsOfType<Canvas>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasFaded)
        {
            hasFaded = true;
            FadeToBlack();
        }
    }

    public void FadeToBlack()
    {
        StartCoroutine(FadeCoroutine(1f));
    }

    public void FadeFromBlack()
    {
        StartCoroutine(FadeCoroutine(0f));
        hasFaded = false;
    }

    private IEnumerator FadeCoroutine(float targetAlpha)
    {
        // Disable all canvases except those with voicelines
        foreach (Canvas canvas in allCanvases)
        {
            if (!canvas.CompareTag("VoiceLines"))
            {
                canvas.enabled = targetAlpha > 0.5f ? false : true;
            }
        }

        Color currentColor = blackOverlay.color;
        float currentAlpha = currentColor.a;

        while (!Mathf.Approximately(currentAlpha, targetAlpha))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
            blackOverlay.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentAlpha);
            yield return null;
        }
    }
}
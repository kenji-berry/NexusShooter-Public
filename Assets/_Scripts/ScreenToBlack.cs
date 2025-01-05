using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenToBlack : MonoBehaviour
{
    [SerializeField] private Image blackOverlay;
    [SerializeField] private float fadeSpeed = 1000f;
    private Canvas[] allCanvases;
    private bool hasFaded = false;

    private void Start()
    {
        // Cache all canvases in the scene
        allCanvases = FindObjectsOfType<Canvas>();
        // Ensure black overlay starts inactive with alpha 0
        blackOverlay.gameObject.SetActive(true);
        Color startColor = blackOverlay.color;
        startColor.a = 0f;
        blackOverlay.color = startColor;
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
        blackOverlay.gameObject.SetActive(true);
        blackOverlay.raycastTarget = true; // Ensure image blocks raycasts

        // Get fresh color reference each time
        Color currentColor = Color.black;
        float currentAlpha = blackOverlay.color.a;

        Debug.Log($"Starting fade from {currentAlpha} to {targetAlpha}");

        while (!Mathf.Approximately(currentAlpha, targetAlpha))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
            currentColor.a = currentAlpha;
            blackOverlay.color = currentColor;
            Debug.Log($"Current alpha: {currentAlpha}");
            yield return null;
        }

        if (Mathf.Approximately(targetAlpha, 0f))
        {
            blackOverlay.gameObject.SetActive(false);
        }
    }
}
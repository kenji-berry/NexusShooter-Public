using System.Collections;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float fadeDuration = 1f; // Duration of the fade-out
    private TextMeshPro textMeshPro;
    private Color originalColor;

    void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
        if (textMeshPro != null)
        {
            originalColor = textMeshPro.color;
        }
    }

    void OnEnable()
    {
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            if (textMeshPro != null)
            {
                textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }
            yield return null;
        }

        Destroy(gameObject);
    }
}

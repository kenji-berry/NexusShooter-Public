using System.Collections;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float moveSpeed = 1f;
    private TextMeshPro textMeshPro;
    private Color targetColor;
    private int fontSize = 12;
    private bool isCritical;

    void Awake()
    {
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    public void Setup(bool isCrit)
    {
        if (textMeshPro != null)
        {
            isCritical = isCrit;
            
            if (isCritical)
            {
                ColorUtility.TryParseHtmlString("#FFD700", out targetColor);
                fontSize = 20;
            }
            else
            {
                targetColor = Color.red;
            }
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = targetColor;
        }
        
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeDuration;
            
            // Fade out
            if (textMeshPro != null)
            {
                textMeshPro.color = new Color(
                    targetColor.r,
                    targetColor.g,
                    targetColor.b,
                    Mathf.Lerp(1f, 0f, normalizedTime)
                );
            }
            
            // Move upward
            transform.position = startPos + (Vector3.up * moveSpeed * normalizedTime);
            
            yield return null;
        }

        Destroy(gameObject);
    }
}

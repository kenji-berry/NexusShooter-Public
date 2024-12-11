using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;

public class VoiceLineTypewriter : MonoBehaviour
{
    public AudioClip[] voiceLines; // Array of voice lines
    public string[] fullTexts; // Array of corresponding texts
    public TMP_Text textComponent; // Reference to the TMP element
    private AudioSource audioSource;
    public float ellipsisPause = 1.0f; // Pause duration for ellipses
    public float commaPause = 0.8f; // Pause duration for commas
    public float fullStopPause = 0.9f; // Pause duration for full stops

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayVoiceAndTypeText());
    }

    IEnumerator PlayVoiceAndTypeText()
    {
        for (int j = 0; j < voiceLines.Length; j++)
        {
            // Start the voice line
            audioSource.clip = voiceLines[j];
            audioSource.Play();

            // Calculate typing speed based on voice line length and number of words
            string[] words = fullTexts[j].Split(' ');
            float typingSpeed = (voiceLines[j].length / words.Length) * 0.5f; // 20% faster

            // Display the text gradually, word by word
            string currentText = "";
            for (int i = 0; i < words.Length; i++)
            {
                currentText += words[i] + " ";
                textComponent.text = currentText;

                // Check for ellipses and pause if found
                if (words[i].Contains("..."))
                {
                    yield return new WaitForSeconds(ellipsisPause);
                }
                // Check for commas and pause if found
                else if (words[i].Contains(","))
                {
                    yield return new WaitForSeconds(commaPause);
                }
                // Check for full stops and pause if found
                else if (words[i].Contains("."))
                {
                    yield return new WaitForSeconds(fullStopPause);
                }
                else
                {
                    yield return new WaitForSeconds(typingSpeed);
                }

                // If the voice line ends early, stop typing
                if (!audioSource.isPlaying)
                    break;
            }

            // Ensure the full text is displayed after the voice finishes
            textComponent.text = fullTexts[j];

            // Wait for the voice line to finish before moving to the next one
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }
    }
}

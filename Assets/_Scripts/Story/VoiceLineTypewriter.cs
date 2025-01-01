using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VoiceLineTypewriter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text voiceLineText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typeSound;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float ellipsisPause = 1.0f;
    [SerializeField] private float commaPause = 0.8f;
    [SerializeField] private float fullStopPause = 0.9f;
    [SerializeField] private float textDisplayTime = 5f;
    [SerializeField] private float questionPause = 1.0f;

    [Header("Voice Lines")]
    [SerializeField] private string[] voiceLines;
    private Queue<int> voiceLineQueue = new Queue<int>();
    public bool isPlaying;
    private Coroutine currentCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        voiceLineText.text = string.Empty;
        
        voiceLines = new string[]
        {    
            "System... malfunction?",
            "This isn't the research lab.",
            "Last thing I remember... the experiment. The teleporter. Something went wrong. Very wrong",
        };
    }

    public void TriggerLine(int index)
    {
        Debug.Log($"Queueing line {index}");
        
        if (index < 0 || index >= voiceLines.Length)
        {
            Debug.LogError($"Invalid voice line index: {index}");
            return;
        }

        voiceLineQueue.Enqueue(index);
        
        // If not already playing, start processing the queue
        if (!isPlaying)
        {
            ProcessNextInQueue();
        }
    }

    private void ProcessNextInQueue()
    {
        if (voiceLineQueue.Count == 0) return;

        int nextIndex = voiceLineQueue.Dequeue();
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(TypeText(voiceLines[nextIndex]));
    }

    private IEnumerator TypeText(string text)
    {
        isPlaying = true;
        Debug.Log($"Starting to type: {text}");
        voiceLineText.text = string.Empty;

        foreach (char c in text)
        {
            voiceLineText.text += c;
            
            if (typeSound != null)
                audioSource.PlayOneShot(typeSound, 0.5f);

            float pause = typingSpeed;
            if (c == '.') pause = fullStopPause;
            else if (c == ',') pause = commaPause;
            else if (c == '?') pause = questionPause;
            else if (c == '.' && text.IndexOf(c) < text.Length - 2 && 
                     text[text.IndexOf(c) + 1] == '.' && 
                     text[text.IndexOf(c) + 2] == '.') 
                pause = ellipsisPause;

            yield return new WaitForSeconds(pause);
        }

        yield return new WaitForSeconds(textDisplayTime);
        voiceLineText.text = string.Empty;
        isPlaying = false;
        currentCoroutine = null;
        Debug.Log("Finished typing line");

        // After finishing the current line, process next in queue
        isPlaying = false;
        ProcessNextInQueue();
    }
}

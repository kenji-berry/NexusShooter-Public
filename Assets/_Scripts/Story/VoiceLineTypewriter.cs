using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private static int currentIndex = 0;
    [SerializeField] private GameObject endElement;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        voiceLineText.text = string.Empty;
        
        voiceLines = new string[]
        {


            "My eyes... am I back? Why can’t I see anything?",
            "These noises... I've heard them before, I must be back!",

            "Johnny, your trial is complete.",
            "Huh?... What trial? Who are you?",
            "Micheal? Is that you? What's going on?",

            "It’s me, yes... but it’s not what you think. This is the end of the cycle.",
            "You created this. Your experiments. You went too far, and it twisted everything.",
            "The mothership... it was your doing. Everything we’ve been through... it’s all part of the madness you unleashed." ,

            "No... no, that can’t be right... I was trying to help.",

            "Dont you feel like you've been here before? You have. You've been here many times. We both have.",
            "This time I will finally end this wretched cycle.",

            "No, please... I can fix this. I can fix everyth-",
            "You can't fix this. You've tried. You've failed",
            "May you rest in peace, Johnny.",
        };

        // Ensure the endElement is hidden on awake
        CanvasGroup canvasGroup = endElement.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = endElement.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f;
    }

    public void TriggerLine()
    {
        Debug.Log($"Queueing line {currentIndex}");
        
        if (currentIndex < 0 || currentIndex >= voiceLines.Length)
        {
            Debug.LogError($"Invalid voice line index: {currentIndex}");
            return;
        }

        voiceLineQueue.Enqueue(currentIndex);
        currentIndex++;
        
        // If not already playing, start processing the queue
        if (!isPlaying)
        {
            ProcessNextInQueue();
        }
    }

    private void ProcessNextInQueue()
    {
        if (voiceLineQueue.Count == 0)
        {
            isPlaying = false;
            return;
        }

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

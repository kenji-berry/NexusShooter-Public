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
            "Ugh... my head... what just happened?",
            "This isn't the research lab... Where am I?",
            "I was working on that strange device and then... everything went black.",
            "If their technology brought me here... there has to be more transporters. A way back to Earth.",

            "A teleporter? It looks functional... maybe it’s my way home.",  

            "This place... it's like nothing I've ever seen before.",
            "The carvings on these walls... they match the patterns on the artifact. This can't be a coincidence.", 
            "This is space? I'm in space? How is that even possible?",
            "I'm breathing... atmosphere control? They were prepared for biological life.",

            "Lava? How is there lava in space?",
            "This place is a death trap. I need to find a way out.",
            "These look like the bounce pads we were working on back at the lab. How are they here?",
            "Looks like they've been modified to work in space. I should be able to use them to get around.",
            "I think I'll need to reach that button to open the door.",
            "Another teleporter, second time's the charm?",

            "I guess not...",
            "How long will this go on for?",
            "I need to find a way out of here... and talk to Micheal about this. He’ll know what’s going on.",
            "If I can make it out...",
            "Is this some kind of sick trial? Otherwise, why would they need to lay it out like this?",
            "Those nightmares I was having... did they mean something?",
            "Another teleporter, third time's the charm? For real this time.",

            "This... this is the place I saw in my nightmares.",
            "The people were alive though and they were... working.",
            "Why can't I remember what they were working on?",
            "...Please... let me go back.",

            "That teleporter... it's different, its linked to earth!",
            "This looks like some kind of sick ritual.",
            "I just have to reach the top then I can go home.",

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
            
            "*The trigger pulls, and the end comes with a sharp, resonant blast.*",
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
            if (currentIndex == voiceLines.Length)
            {
                StartCoroutine(FadeInEndElement());
                Debug.Log("End element faded in");
            }
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

        // Start fading in the end element
        if (!isPlaying)
        {
            StartCoroutine(FadeInEndElement());
        }
    }

    private IEnumerator FadeInEndElement()
    {
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
}

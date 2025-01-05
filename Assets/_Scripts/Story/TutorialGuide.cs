using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TutorialGuide : MonoBehaviour
{
    public TMP_Text tutorialText; // Reference to the TMP element
    private Dictionary<string, string> tutorialMessages; // Dictionary to store tutorial messages

    void Start()
    {
        // Initialize tutorial messages
        tutorialMessages = new Dictionary<string, string>
        {
            { "Move", "Use WASD to move" },
            { "Jump", "Press Space to jump" },
            { "Crouch", "Press C to crouch" },
            { "Interact", "Press E to interact with objects" },
            { "Attack", "Press LMB to attack with your equipped weapon" },
            { "Equip", "Press Q to open the weapon inventory or use the number keys to switch weapons quickly" },
            { "Clear", "" }
        };

    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered a tutorial trigger

        if (other.CompareTag("TutorialTrigger"))
        {
            Debug.Log("Player entered tutorial trigger");
            // Get the tutorial message key from the trigger object
            string messageKey = other.gameObject.name;

            // Display the corresponding tutorial message
            if (tutorialMessages.ContainsKey(messageKey))
            {
                tutorialText.text = tutorialMessages[messageKey];
            }
        }
    }

    void OnTriggerExit(Collider other)
    {

    }
}
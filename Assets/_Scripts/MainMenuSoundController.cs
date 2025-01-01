using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClick;
    public AudioClip buttonHover;
    private static bool isSoundEnabled = true; // Global sound state

    public void Play(AudioClip clip, float volume = 1f)
    {
        if (!isSoundEnabled) return;
        audioSource.PlayOneShot(clip, volume);
    }

    public static void ToggleSound(bool enabled)
    {
        isSoundEnabled = enabled;
    }

    public static bool IsSoundEnabled()
    {
        return isSoundEnabled;
    }
}

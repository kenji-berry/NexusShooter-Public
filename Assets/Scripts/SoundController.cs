using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shoot;
    public AudioClip melee;
    public AudioClip hit;
    public AudioClip jump;

    private static bool isSoundEnabled = false; // Global sound state

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

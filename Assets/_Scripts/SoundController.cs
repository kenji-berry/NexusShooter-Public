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
    public AudioClip levelUp;
    public AudioClip skillPointPurchase;
    public AudioClip buttonClick;
    public AudioClip buttonHover;
    public AudioClip playerDamage1;
    public AudioClip playerDamage2;
    public AudioClip playerDamage3;
    public AudioClip playerDamage4;
    public AudioClip playerDamage5;
    public AudioClip playerDamage6;
    public AudioClip getHit;
    public AudioClip criticalHit;
    public AudioClip doorButtonPress;
    public AudioClip splat;
    public AudioClip finalShot;


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

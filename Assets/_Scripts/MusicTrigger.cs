using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public AudioClip[] musicTracks;  // Array to hold different music tracks
    public AudioSource audioSource; // The AudioSource that plays the music
    private AudioClip lastPlayedTrack; // To store the last played track

    void Start()
    {
        audioSource = FindFirstObjectByType<AudioSource>();  // Find the AudioSource component in the scene
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Check if the player enters the area
        {
            PlayRandomMusic();
        }
    }

    void PlayRandomMusic()
    {
        if (musicTracks.Length > 0)
        {
            AudioClip selectedTrack = GetRandomTrack();

            // Set the AudioSource's clip and play it
            audioSource.clip = selectedTrack;
            audioSource.Play();
            lastPlayedTrack = selectedTrack; // Update the last played track
        }
    }

    AudioClip GetRandomTrack()
    {
        AudioClip randomTrack;

        do
        {
            randomTrack = musicTracks[Random.Range(0, musicTracks.Length)];
        }
        while (randomTrack == lastPlayedTrack);  // Ensure the new track is different from the last one

        return randomTrack;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Stop();  // Stop the music when the player exits the area
        }
    }
}

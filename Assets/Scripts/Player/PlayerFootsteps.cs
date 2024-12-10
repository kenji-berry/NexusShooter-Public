using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private PlayerController playerController;
    private AudioSource audioSource;

    public AudioClip footstepSound;
    public float maxFootstepInterval = 0.5f; // Maximum interval between footsteps

    private float footstepTimer;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

        if (playerController == null)
        {
            Debug.LogError("PlayerController component not found on the player.");
        }

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the player.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController != null && audioSource != null)
        {
            Vector3 horizontalVelocity = new Vector3(playerController.characterVelocity.x, 0, playerController.characterVelocity.z);
            if (horizontalVelocity.magnitude > 0.1f && playerController.isGrounded)
            {
                footstepTimer -= Time.deltaTime;
                if (footstepTimer <= 0f)
                {
                    PlayFootstep();
                    footstepTimer = Random.Range(0.2f, maxFootstepInterval);
                }
            }
            else
            {
                footstepTimer = 0f;
            }
        }
    }

    private void PlayFootstep()
    {
        audioSource.PlayOneShot(footstepSound);
    }
}

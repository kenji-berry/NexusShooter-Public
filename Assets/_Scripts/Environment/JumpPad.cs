using UnityEngine;
public class JumpPad : MonoBehaviour
{
    [Header("Jump Pad Settings")]

    public float jumpPadUpwardForce = 30f;
    public float horizontalBoostMultiplier = 2.5f;

    public AudioClip jumpPadSound;

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other.name);
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.ForceDirectionalLaunch(jumpPadUpwardForce, horizontalBoostMultiplier);

            if (jumpPadSound && TryGetComponent<AudioSource>(out AudioSource audioSource))
            {
                audioSource.PlayOneShot(jumpPadSound);
            }
        }
    }
}
using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    [SerializeField] private VoiceLineTypewriter typewriter;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            typewriter.TriggerLine();
            hasTriggered = true;
            gameObject.SetActive(false);
        }
    }
}
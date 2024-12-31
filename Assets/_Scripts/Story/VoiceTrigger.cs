using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    [SerializeField] private VoiceLineTypewriter typewriter;
    [SerializeField] private int lineIndex;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            typewriter.TriggerLine(lineIndex);
            hasTriggered = true;
            gameObject.SetActive(false);
        }
    }
}
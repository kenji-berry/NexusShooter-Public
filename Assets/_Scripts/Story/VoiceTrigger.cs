using UnityEngine;

public class VoiceTrigger : MonoBehaviour
{
    [SerializeField] private VoiceLineTypewriter typewriter;
    private bool hasTriggered = false;
    private void Awake()
    {
        if (typewriter == null)
        {
            GameObject soundController = GameObject.FindGameObjectWithTag("Audio");
            if (soundController != null)
            {
                typewriter = soundController.GetComponent<VoiceLineTypewriter>();
            }
        }
    }
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
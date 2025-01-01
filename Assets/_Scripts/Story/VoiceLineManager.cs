using UnityEngine;

public class VoiceLineManager : MonoBehaviour
{
    public static VoiceLineManager Instance { get; private set; }
    private int currentVoiceLineIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementIndex()
    {
        currentVoiceLineIndex++;
        Debug.Log($"Voice line index: {currentVoiceLineIndex}");
    }

    public int GetCurrentIndex()
    {
        return currentVoiceLineIndex;
    }
}
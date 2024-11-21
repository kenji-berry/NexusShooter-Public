using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public GameObject deathScreen;

    private float startTime;
    private float endTime;
    private bool levelCompleted = false;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (!levelCompleted) {
            float timeTaken = Time.time - startTime;
            timer.text = FormatTime(timeTaken);
        }
    }

    public void CompleteLevel()
    {
        if (!levelCompleted)
        {
            endTime = Time.time;
            levelCompleted = true;

            float timeTaken = endTime - startTime;
            timer.text = FormatTime(timeTaken);
        }
    }

    public void Die()
    {
        deathScreen.SetActive(true);
    }

    private string FormatTime(float time)
    {
        int minutes = (int) time / 60;
        int seconds = (int) time % 60;
        int milliseconds = (int) (time * 100) % 100;
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}

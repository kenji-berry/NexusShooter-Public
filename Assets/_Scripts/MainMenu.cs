using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject difficultyPanel;

    private void Awake()
    {
        playPanel = GameObject.Find("PlayScreen");
        difficultyPanel = GameObject.Find("DifficultyScreen");
        difficultyPanel.SetActive(false);
    }

    public void StartGame()
    {
        playPanel.SetActive(false);
        difficultyPanel.SetActive(true);
    }

    public void LoadEasy()
    {
        SceneManager.LoadScene("1- Easy");
    }

    public void LoadMedium()
    {
        SceneManager.LoadScene("2- Medium");
    }

    public void LoadHard()
    {
        SceneManager.LoadScene("3- Hard");
    }

    public void ReturnToMenu()
    {
        if (playPanel != null)
        {
            playPanel.SetActive(true);
        }
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(false);
        }
    }
}
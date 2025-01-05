using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject loadSlotsPanel;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject newGameSlotsPanel;
    [SerializeField] private SoundController soundController;

    private void Awake()
    {
        ValidateReferences();
        SetupInitialState();
    }

    private void ValidateReferences()
    {
        if (playPanel == null)
            Debug.LogError($"Play Panel not assigned on {gameObject.name}");
            
        if (difficultyPanel == null)
            Debug.LogError($"Difficulty Panel not assigned on {gameObject.name}");
            
        if (soundController == null)
            Debug.LogError($"Sound Controller not assigned on {gameObject.name}");
    }

    private void SetupInitialState()
    {
        if (difficultyPanel != null)
            difficultyPanel.SetActive(false);
    }

    public void StartGame()
    {
        if (playPanel != null)
            playPanel.SetActive(false);
        if (difficultyPanel != null)
            difficultyPanel.SetActive(true);
    }

    public void LoadEasy()
    {
        SceneManager.LoadScene("1 - Easy");
    }

    public void LoadMedium()
    {
        SceneManager.LoadScene("2 - Medium");
    }

    public void LoadHard()
    {
        SceneManager.LoadScene("3 - Hard");
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

    public void buttonClick()
    {
        soundController.Play(soundController.buttonClick);
    }

    public void buttonHover()
    {
        soundController.Play(soundController.buttonHover);
    }

    public void NewSave(int slot)
    {
        SaveSystem.ClearSave(slot);
        SaveSystem.saveSlot = slot;

        ProceedToDifficultMenu();
    }

    public void SaveLoad(int slot)
    {
        SaveSystem.saveSlot = slot;

        ProceedToDifficultMenu();
    }

    public void OpenLoadSlotsMenu()
    {
        playPanel.SetActive(false);
        loadSlotsPanel.SetActive(true);
    }

    public void CloseLoadSlotsMenu()
    {
        loadSlotsPanel.SetActive(false);
        playPanel.SetActive(true);
    }

    public void OpenNewGameSlotsMenu()
    {
        playPanel.SetActive(false);
        newGameSlotsPanel.SetActive(true);
    }

    public void CloseNewGameSlotsMenu()
    {
        newGameSlotsPanel.SetActive(false);
        playPanel.SetActive(true);
    }

    private void ProceedToDifficultMenu()
    {
        if (loadSlotsPanel.activeInHierarchy)
        {
            loadSlotsPanel.SetActive(false);
        }

        if (newGameSlotsPanel.activeInHierarchy)
        {
            newGameSlotsPanel.SetActive(false);
        }

        difficultyPanel.SetActive(true);
    }
}
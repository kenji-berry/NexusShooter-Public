using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public GameObject deathScreen;
    public GameObject pauseMenu;
    public GameObject settingsPanel; // Reference to Settings Panel
    public GameObject skillTreePanel; // Reference to Skill Tree Panel
    public Slider mouseSensitivitySlider;
    public Slider volumeSlider;
    public AudioSource audioSource;
    private float startTime;
    private float endTime;
    private bool levelCompleted = false;

    public bool isPaused = false;
    private PlayerController playerController;
    private InputManager inputManager; // Reference the generated InputManager


    void Awake(){
        playerController = FindObjectOfType<PlayerController>();
        pauseMenu.SetActive(false);
        settingsPanel.SetActive(false);
        mouseSensitivitySlider.value = playerController.mouseSensitivity;
        mouseSensitivitySlider.onValueChanged.AddListener(UpdateMouseSensitivity);
    }


    void Start()
    {
        startTime = Time.time;


        if (audioSource != null)
        {
            audioSource.volume = 1.0f; // Set volume to max
        }


        if (volumeSlider != null && audioSource != null)
        {
            volumeSlider.value = audioSource.volume * 10f; // Set slider to match audio source volume (convert 0-1 to 0-10)
            volumeSlider.onValueChanged.AddListener(UpdateVolume); // Add listener to slider
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (!levelCompleted && !isPaused)
        {
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

   void OnTooglePauseMenu(InputValue value) // Automatically linked
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

   private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Freeze game time
        pauseMenu.SetActive(true);

        // Disable player controls
        playerController.inventoryOpen = true; // Blocks inventory and movement in PlayerController
        playerController.enabled = false; // Disables PlayerController update loop

            // Close inventory UI
        var weaponsManager = FindObjectOfType<WeaponsManager>();
        if (weaponsManager != null && weaponsManager.inventoryUI.activeInHierarchy)
        {
            weaponsManager.inventoryUI.SetActive(false); // Hide the inventory
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume game time
        pauseMenu.SetActive(false);
        settingsPanel.SetActive(false);

        // Re-enable player controls
        playerController.inventoryOpen = false; // Re-enables inventory and movement
        playerController.enabled = true; // Re-enables PlayerController update loop
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettingsMenu()
    {
        pauseMenu.SetActive(false);   
        settingsPanel.SetActive(true); 
        Debug.Log("Audio Source Volume: " + audioSource.volume);
    }

    public void CloseSettingsMenu()
    {
        settingsPanel.SetActive(false); 
        pauseMenu.SetActive(true);
        Debug.Log("Audio Source Volume: " + audioSource.volume);
    }

    public void OpenSkillTreeMenu()
    {
        pauseMenu.SetActive(false);   
        skillTreePanel.SetActive(true); 
    }

    public void CloseSkillTreeMenu()
    {
        skillTreePanel.SetActive(false); 
        pauseMenu.SetActive(true);
    }

    public void MainMenu(){
        Debug.Log("Loading Main Menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateMouseSensitivity(float sensitivity)
    {
        if (playerController != null)
        {
            playerController.SetMouseSensitivity(sensitivity);
        }
    }

    public void UpdateVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume / 10f; // Convert slider value to 0-1 range
            Debug.Log("Volume set to: " + audioSource.volume);
            Debug.Log("Slider value: " + volumeSlider.value);
        }
    }
}

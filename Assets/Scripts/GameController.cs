using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GameController : MonoBehaviour
{
    // UI Elements
    public TextMeshProUGUI timer;
    public GameObject deathScreen;
    public GameObject pauseMenu;
    public GameObject settingsPanel;
    public GameObject skillTreePanel;
    public Slider mouseSensitivitySlider;
    public Slider volumeSlider;
    public Slider fovSlider;
    public TMP_Dropdown colorBlindnessDropdown;

    // Audio
    public AudioSource audioSource;

    // Camera
    public Camera playerCamera;

    // Post Processing
    public PostProcessVolume postProcessVolume;
    public PostProcessProfile normalProfile;
    public PostProcessProfile deuteranopiaProfile;
    public PostProcessProfile protanopiaProfile;
    public PostProcessProfile tritanopiaProfile;

    // Game State
    private float startTime;
    private float endTime;
    private bool levelCompleted = false;
    public bool isPaused = false;

    // References
    private PlayerController playerController;
    private InputManager inputManager;

    void Awake()
    {
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
            audioSource.volume = 1.0f;
        }

        if (volumeSlider != null && audioSource != null)
        {
            volumeSlider.value = audioSource.volume * 10f;
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
        }

        if (playerCamera != null && fovSlider != null)
        {
            float normalizedFOV = (playerCamera.fieldOfView - 70f) / (120f - 70f);
            fovSlider.value = normalizedFOV * fovSlider.maxValue;
            fovSlider.onValueChanged.AddListener(UpdateFOV);
        }

        if (colorBlindnessDropdown != null)
        {
            colorBlindnessDropdown.ClearOptions();
            var options = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("Normal"),
                new TMP_Dropdown.OptionData("Deuteranopia"),
                new TMP_Dropdown.OptionData("Protanopia"),
                new TMP_Dropdown.OptionData("Tritanopia")
            };
            colorBlindnessDropdown.AddOptions(options);
            colorBlindnessDropdown.onValueChanged.AddListener(UpdateColorBlindnessFilter);
        }
    }

    void Update()
    {
        if (!levelCompleted && !isPaused) {
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
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int milliseconds = (int)(time * 100) % 100;
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    void OnTooglePauseMenu(InputValue value)
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
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        playerController.inventoryOpen = true;
        playerController.enabled = false;

        var weaponsManager = FindObjectOfType<WeaponsManager>();
        if (weaponsManager != null && weaponsManager.inventoryUI.activeInHierarchy)
        {
            weaponsManager.inventoryUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        settingsPanel.SetActive(false);
        playerController.inventoryOpen = false;
        playerController.enabled = true;
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

    public void MainMenu()
    {
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
            audioSource.volume = volume / 10f;
            Debug.Log("Volume set to: " + audioSource.volume);
            Debug.Log("Slider value: " + volumeSlider.value);
        }
    }

    public void UpdateFOV(float sliderValue)
    {
        if (playerCamera != null)
        {
            float fov = Mathf.Lerp(80f, 120f, sliderValue / fovSlider.maxValue);
            playerCamera.fieldOfView = fov;
            Debug.Log("FOV set to: " + fov);
        }
    }

    public void UpdateColorBlindnessFilter(int index)
    {
        switch (index)
        {
            case 0:
                postProcessVolume.profile = normalProfile;
                break;
            case 1:
                postProcessVolume.profile = deuteranopiaProfile;
                break;
            case 2:
                postProcessVolume.profile = protanopiaProfile;
                break;
            case 3:
                postProcessVolume.profile = tritanopiaProfile;
                break;
            default:
                postProcessVolume.profile = normalProfile;
                break;
        }

        Debug.Log("Color blindness filter set to: " + colorBlindnessDropdown.options[index].text);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public GameObject deathScreen;

    private float startTime;
    private float endTime;
    private bool levelCompleted = false;

    public bool isPaused = false;
    private PlayerController playerController;
    private InputManager inputManager; // Reference the generated InputManager


    void Awake(){
    playerController = FindObjectOfType<PlayerController>();
    }


    void Start()
    {
        startTime = Time.time;
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
        // pauseMenu.SetActive(true);

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

    private void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resume game time
        // pauseMenu.SetActive(false);

        // Re-enable player controls
        playerController.inventoryOpen = false; // Re-enables inventory and movement
        playerController.enabled = true; // Re-enables PlayerController update loop
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

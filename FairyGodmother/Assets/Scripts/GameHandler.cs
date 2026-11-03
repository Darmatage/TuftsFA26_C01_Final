using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class GameHandler : MonoBehaviour
{
    private GameObject player;
    public GameInventory inventory; 
    private bool isPaused = false;
    public GameObject pauseMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player");
            if (player != null) {

                pauseMenu.SetActive(false);
            }
    }

    // Update is called once per frame
    void Update()
    {
        //manage pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused) {
                ResumeGame();
            } else {
                PauseGame();
            }
        }

        //manage inventory
        if (Input.GetKeyDown(KeyCode.I))
            inventory.OpenCloseInventory();

        if (Input.GetKeyDown(KeyCode.P))
        {
            inventory.InventoryAdd("apple");
            Debug.Log("apple added");
        }
    }

    public void StartGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MapScene");
    }

    public void MainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void Credits() {
        SceneManager.LoadScene("Credits");
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        //time needs to be set to 1 before setting pauseMenu Active
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenu.SetActive(false);
    }

    public void WinGame()
    {
        SceneManager.LoadScene("WinScene");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

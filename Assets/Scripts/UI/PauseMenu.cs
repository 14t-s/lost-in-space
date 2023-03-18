using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public static bool isPaused;
    public static bool isOptions;

    private Controls playerControlsAction;




    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);

        playerControlsAction = new Controls();



        playerInput = GetComponent<PlayerInput>();
    }
    /**
    private void OnEnable()
    {
        playerControlsAction.Enable();
    }
    private void OnDisable()
    {
        playerControlsAction.Disable();
    }*/

    // Update is called once per frame
    void Update()
    {
        /**
        bool pressPause = playerControlsAction.Gameplay.OpenPauseMenu.ReadValue<bool>();
        
        if (pressPause == true)
        {
            if (isOptions)
            {
                CloseOptions();
            }
            else if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }*/

        //Debug.Log(playerInput.currentActionMap);
    }

    private void SwitchActionMapPaused()
    {
        //playerInput.SwitchCurrentActionMap("Paused");
        playerControlsAction.Paused.Enable();
        playerControlsAction.Gameplay.Disable();
    }
    private void SwitchActionMapResume()
    {
        //playerInput.SwitchCurrentActionMap("Gameplay");
        playerControlsAction.Gameplay.Enable();
        playerControlsAction.Paused.Disable();
    }

    public void PauseGame()
    {
        playerInput.SwitchCurrentActionMap("Paused");
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        
        //SwitchActionMapPaused();

    }

    public void ResumeGame()
    {
        playerInput.SwitchCurrentActionMap("Gameplay");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        
        //SwitchActionMapResume();
    }

    // Options menu
    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
        isOptions = true;
    }

    public void CloseOptions()
    {
        optionsMenu.SetActive(false);
        isOptions = false;
    }

    // Quit to main menu
    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

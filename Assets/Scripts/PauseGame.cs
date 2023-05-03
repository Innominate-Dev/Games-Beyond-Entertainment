using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public bool GameIsPaused = false;
    public GameObject InteractManager;
    public GameObject pauseMenuUI;
    public RawImage Interaction;


    public void pause_system()
    {
        if (Time.timeScale == 0)
        {
            pauseMenuUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            GameIsPaused = false;
            InteractManager.SetActive(true);
            Interaction.gameObject.SetActive(true);
        }
        else
        {
            GameIsPaused = true;
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            InteractManager.SetActive(false);
            Interaction.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause_system();
        }
    }
}

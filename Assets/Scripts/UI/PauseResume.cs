using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseResume : MonoBehaviour
{
    public GameObject PauseScreen;
    public KeyCode interactKey = KeyCode.Escape;
    bool GamePaused;

    void Start()
    {
        GamePaused = false;
        PauseScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (GamePaused)
                ResumeGame();
            else
                PauseGame();
        }

        Time.timeScale = GamePaused ? 0 : 1;
    }

    public void PauseGame()
    {
        GamePaused = true;
        PauseScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        GamePaused = false;
        PauseScreen.SetActive(false);
    }
}
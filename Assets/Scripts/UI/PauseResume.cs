using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseResume : MonoBehaviour
{
    public GameObject PauseScreen;
    public KeyCode ResumeKey = KeyCode.Escape;
    public KeyCode ResetKey = KeyCode.R;
    bool GamePaused;

    void Start()
    {
        GamePaused = false;
        PauseScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(ResumeKey))
        {
            if (GamePaused)
                ResumeGame();
            else
                PauseGame();
        }

        Time.timeScale = GamePaused ? 0 : 1;

        if (Input.GetKeyDown(ResetKey))
        {
            Reset();
        }
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

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }
}

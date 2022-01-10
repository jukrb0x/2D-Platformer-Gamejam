using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool IsPaused { get; set; }
    public bool IsWon { get; set; }

    private GameController gameController; // will be added to the GameObject automatically
    private Collider2D player;

    private void Start()
    {
        gameController = GetComponent<GameController>();
        IsPaused = false;
        // make frame per second 45
        Application.targetFrameRate = 45;
    }

    public void EnterNextLevel()
    {
        OnLoadTheScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnLoadTheScene()
    {
        Resume();
    }

    public void Pause(bool cursor = true)
    {
        IsPaused = true;
        gameController.PauseTheGame();
        Cursor.visible = cursor;
    }

    public void Resume(bool cursor = false)
    {
        IsPaused = false;
        gameController.ResumeTheGame();
        Cursor.visible = cursor;
    }
}
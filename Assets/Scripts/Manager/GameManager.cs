using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool IsPaused { get; set; }

    public bool IsWon { get; set; }

    private GameController gameController; // will be added to the GameObject automatically

    private Collider2D player;

    private void Awake()
    {
        // gameController = GetComponent<GameController>();
        gameObject.AddComponent<GameController>();
    }

    private void Start()
    {
        gameController = GetComponent<GameController>();
        IsPaused = false;
    }

    private void Update()
    {
        
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

    public void Pause()
    {
        IsPaused = true;
        gameController.PauseTheGame();
    }

    public void Resume()
    {
        IsPaused = false;
        gameController.ResumeTheGame();
    }
}
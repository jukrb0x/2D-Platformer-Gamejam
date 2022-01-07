using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        // make cursor visible
        Cursor.visible = true;
        gameManager = FindObjectOfType<GameManager>();
    }

    public void StartTheGame()
    {
        gameManager.OnLoadTheScene();
        SceneManager.LoadScene(1);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
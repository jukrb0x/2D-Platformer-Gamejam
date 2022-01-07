using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    private void Start()
    {
        // make cursor visible
        Cursor.visible = true;
    }

    public void StartTheGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
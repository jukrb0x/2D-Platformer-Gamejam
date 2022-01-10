using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void PauseTheGame()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        AudioListener.volume = 0;
    }

    public void ResumeTheGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        AudioListener.volume = 1;
    }
    
}
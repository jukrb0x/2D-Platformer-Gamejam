using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void PauseTheGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeTheGame()
    {
        Time.timeScale = 1;
    }
    
}
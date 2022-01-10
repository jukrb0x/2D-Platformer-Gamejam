using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameData : MonoBehaviour
{
    public int PrevScore;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    
    public void SaveScore(int value)
    {
        // PlayerPrefs.SetInt("PrevScore", PrevScore);
        PrevScore = value;
    }

    public int ReadScore()
    {
        return PrevScore;
    }
}
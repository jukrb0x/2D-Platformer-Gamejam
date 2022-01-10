using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager ??= FindObjectOfType<GameManager>();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        gameManager.OnLoadTheScene();
        SceneManager.LoadScene(1);
    }
}
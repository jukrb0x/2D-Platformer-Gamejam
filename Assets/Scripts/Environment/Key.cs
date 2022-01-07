using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // TODO pass level
        if (col.CompareTag("Player"))
        {
            gameManager.IsWon = true;
        }
    }
}
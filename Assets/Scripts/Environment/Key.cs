using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    private GameManager gameManager;
    private KeyType _keyType;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (_keyType == KeyType.winnerKey)
            gameManager.IsWon = true;
    }

    enum KeyType
    {
        winnerKey,
        nextLevelKey
    }
}
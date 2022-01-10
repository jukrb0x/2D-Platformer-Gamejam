using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private KeyType _keyType;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (_keyType == KeyType.winnerKey)
            gameManager.IsWon = true;
        if (_keyType == KeyType.nextLevelKey)
        {
            GameObject.Find("Game Data").GetComponent<GameData>().SaveScore(gameManager.Score);
            gameManager.EnterNextLevel();
        }
    }

    enum KeyType
    {
        winnerKey,
        nextLevelKey
    }
}
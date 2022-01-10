using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private CoinType _coinType;

    private void Start()
    {
        gameManager ??= FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (_coinType == CoinType.startCoin)
            {
                gameManager.OnLoadTheScene();
                SceneManager.LoadScene(1);
            }
            else if (_coinType == CoinType.scoreCoin)
            {
                gameManager.Score += 3; // add score to player
                Destroy(gameObject);
            }
        }
    }

    enum CoinType
    {
        startCoin,
        scoreCoin,
    }
}
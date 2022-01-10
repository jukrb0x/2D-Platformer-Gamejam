using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager ??= FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            gameManager.IsDead = true;
        }
        else
        {
            Destroy(col.gameObject);
        }
    }
}
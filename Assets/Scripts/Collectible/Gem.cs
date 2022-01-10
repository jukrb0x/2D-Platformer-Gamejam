using System;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private StartMenuController startMenuController;
    private GameManager gameManager;

    private void Start()
    {
        startMenuController ??= FindObjectOfType<StartMenuController>();
        gameManager ??= FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        startMenuController.ShowQuitContirm();
        gameManager.Pause(true);
    }
}
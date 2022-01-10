using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private GameManager gameManager;
    private UIManager uiManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        uiManager ??= GameObject.Find("HUD Canvas").GetComponent<UIManager>();
    }

    public void ContinueOnClick()
    {
        gameManager.Resume();
        uiManager.HidePauseMenu();
    }

    public void ExitOnClick()
    {
        // back to menu
        gameManager.OnLoadTheScene();
        SceneManager.LoadSceneAsync(0);
    }
}
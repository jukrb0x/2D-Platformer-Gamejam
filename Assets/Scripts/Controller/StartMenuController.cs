using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject quitConfirm;
    private bool isQuitConfirmOpen;

    private void Start()
    {
        // make cursor visible
        Cursor.visible = true;
        gameManager ??= FindObjectOfType<GameManager>();
        quitConfirm ??= gameObject.transform.Find("Quit Confirm").gameObject;
        quitConfirm.SetActive(false);
        isQuitConfirmOpen = false;
    }

    private void Update()
    {
        if (isQuitConfirmOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            HideQuitConfirm();
        }
    }

    public void StartTheGame()
    {
        gameManager.OnLoadTheScene();
        SceneManager.LoadScene(1);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    public void ShowQuitContirm()
    {
        quitConfirm.SetActive(true);
        isQuitConfirmOpen = true;
    }

    public void HideQuitConfirm()
    {
        quitConfirm.SetActive(false);
        isQuitConfirmOpen = false;
        gameManager.Resume(true);
    }
}
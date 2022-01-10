using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private HealthBarController healthBar;
    private PowerBarController powerBar;
    private CharacterController2D player;
    private GameObject pauseMenu;
    private GameObject winnerMenu;
    [SerializeField] private GameManager gameManager;


    private void Awake()
    {
        gameManager ??= GameObject.Find("Game Manager").GetComponent<GameManager>();
        healthBar ??= GameObject.Find("Health Bar").GetComponent<HealthBarController>();
        powerBar ??= GameObject.Find("Power Bar").GetComponent<PowerBarController>();
        player ??= GameObject.Find("Player").GetComponent<CharacterController2D>();
        pauseMenu ??= GameObject.Find("Pause Menu");
        winnerMenu ??= GameObject.Find("Winner Menu");
    }

    private void Start()
    {
        healthBar.SetMaxHealth(player.life);
        powerBar.SetMaxPower(player.power);
        pauseMenu.SetActive(false);
        winnerMenu.SetActive(false);
    }

    private void Update()
    {
        UpdateHealthBar(player.life);
        UpdatePowerBar(player.power);

        // receive keyboard input

        // isWon -> winnerMenu
        // TODO isDead -> gameOverMenu
        if (!gameManager.IsWon && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameManager.IsPaused)
            {
                gameManager.Pause();
                ShowPauseMenu();
            }
            else if (gameManager.IsPaused)
            {
                gameManager.Resume();
                HidePauseMenu();
            }
        }
        else if (gameManager.IsWon)
        {
            gameManager.Pause();
            ShowWinnerMenu();
        }
    }

    // routines
    private void UpdateHealthBar(float health)
    {
        healthBar.SetHealth(health);
    }

    private void UpdatePowerBar(float power)
    {
        powerBar.SetPower(power);
    }

    // pause menu
    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    // win the game
    private void ShowWinnerMenu()
    {
        winnerMenu.SetActive(true);
    }
}
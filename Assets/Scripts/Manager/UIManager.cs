using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private HealthBarController healthBar;
    private PowerBarController powerBar;
    private CharacterController2D player;
    private GameObject pauseMenu;
    private GameObject winnerMenu;
    private GameObject gameOverMenu;
    private TextMeshProUGUI scoreBar;
    [SerializeField] private GameManager gameManager;


    private void Awake()
    {
        gameManager ??= GameObject.Find("Game Manager").GetComponent<GameManager>();
        healthBar ??= GameObject.Find("Health Bar").GetComponent<HealthBarController>();
        powerBar ??= GameObject.Find("Power Bar").GetComponent<PowerBarController>();
        player ??= GameObject.Find("Player").GetComponent<CharacterController2D>(); //todo tag
        pauseMenu ??= GameObject.Find("Pause Menu");
        winnerMenu ??= GameObject.Find("Winner Menu");
        gameOverMenu ??= GameObject.Find("GameOver Menu");
        scoreBar ??= GameObject.Find("Score Bar").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        healthBar.SetMaxHealth(player.life);
        powerBar.SetMaxPower(player.power);
        pauseMenu.SetActive(false);
        winnerMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    private void Update()
    {
        UpdateHealthBar(player.life);
        UpdatePowerBar(player.power);
        UpdateScoreBar(gameManager.Score);
        // receive keyboard input

        // isWon -> winnerMenu
        // TODO isDead -> gameOverMenu
        if (!gameManager.IsDead && !gameManager.IsWon && Input.GetKeyDown(KeyCode.Escape))
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
        else if (gameManager.IsDead)
        {
            gameManager.Pause();
            ShowGameOverMenu();
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

    private void UpdateScoreBar(int score)
    {
        scoreBar.text = $"Score: {score}";
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
        SetScoreOnUI();
    }

    private void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        SetScoreOnUI();
    }

    private void SetScoreOnUI()
    {
        TextMeshProUGUI scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        if (scoreText != null) scoreText.text = $"Score: {gameManager.Score}";
    }
}
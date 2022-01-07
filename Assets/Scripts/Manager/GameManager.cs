using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isPaused;
    private bool isWon;
    
    // Win the game
    // Fail the game
    // Exit the game

    private Collider2D player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
    }

    private void Update()
    {
        // TODO delete me
        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     player.gameObject.GetComponent<CharacterController2D>()
        //         .ApplyDamage(2f, transform.position);
        // }
    }
    public void EnterNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        if (Input.GetKeyDown(KeyCode.I))
        {
                player.gameObject.GetComponent<CharacterController2D>()
                    .ApplyDamage(2f, transform.position);
        }
    }
}
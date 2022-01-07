using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private HealthBarController healthBar;
    private CharacterController2D player;

    private void Awake()
    {
        healthBar ??= GameObject.Find("Health Bar").GetComponent<HealthBarController>();
        player ??= GameObject.Find("Player").GetComponent<CharacterController2D>();
    }

    private void Start()
    {
        // TODO
        healthBar.SetMaxHealth(player.life);
    }

    private void Update()
    {
        UpdateHealthBar(player.life);
    }

    private void UpdateHealthBar(float health)
    {
        healthBar.SetHealth(health);
    }
    
}
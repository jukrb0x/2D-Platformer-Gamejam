using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class HealthBottle : MonoBehaviour
{
    private CharacterController2D player;
    [SerializeField] private float volume = 3f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            player.AddHealth(volume);
            Destroy(gameObject);
        }
    }
}
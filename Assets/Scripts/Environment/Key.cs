using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        // TODO pass level
        if (col.CompareTag("Player"))
        {
            print("you win");
        }
    }
}
using System;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private float dmgValue = 2f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.GetComponent<CharacterController2D>().ApplyDamage(dmgValue, transform.position);
        }
    }
}
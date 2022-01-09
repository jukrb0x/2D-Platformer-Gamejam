using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 400f;

    float horizontalMoveSpeed = 0f;
    bool jump = false;
    bool dash = false;
    [SerializeField] private bool canDash = false; // Player Dash Switch
    [SerializeField] private GameManager gameManager;

    // bool dashAxis = false;

    private void Start()
    {
        gameManager ??= GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        if (gameManager.IsPaused) return;
        
        // Move player
        controller.Move(horizontalMoveSpeed * Time.fixedDeltaTime, jump, dash);
        // after move in each physics calc
        jump = false;
        dash = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMoveSpeed = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMoveSpeed));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            dash = true;
        }

        /*if (Input.GetAxisRaw("Dash") == 1 || Input.GetAxisRaw("Dash") == -1) //RT in Unity 2017 = -1, RT in Unity 2019 = 1
        {
            if (dashAxis == false)
            {
                dashAxis = true;
                dash = true;
            }
        }
        else
        {
            dashAxis = false;
        }
        */
    }

    // Hooks
    public void OnFall()
    {
        animator.SetBool("IsJumping", true);
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }
}
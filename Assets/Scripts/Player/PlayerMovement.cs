using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator; // seems to be found itself on the component

    public float runSpeed = 40f;

    float horizontalMove = 0f;

    // action is executed: true
    bool jump = false;
    bool dash = false;

    //bool dashAxis = false;

    // Update is called once per frame
    void Update()
    {
        // receive user input on every frame updated

        // GetAxisRaw only returns 
        // -1, 0, 1
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
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

    public void OnFall()
    {
        animator.SetBool("IsJumping", true);
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    private void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);
        jump = false;
        dash = false;
    }
}
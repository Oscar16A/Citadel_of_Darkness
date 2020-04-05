﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    public float speed = 6f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 3f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public float fallingGravityMultiplier = 2f;
    private float startGravity;
    Vector3 velocity;
    public bool airControl;
    Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    [Header("Animation Queues")]
    public bool isGrounded;
    public bool walking;
    public bool freeze = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("Ground Check");
        startGravity = gravity;
    }
    void Update()
    {
        if(!freeze)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            walking = false;
            //Debug.Log(controller.velocity);
            if(Vector3.Magnitude(controller.velocity) > 0 && isGrounded)
            {
                walking = true;
            }

            if(isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            //horizontal movement
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            move = Vector3.ClampMagnitude(move, 1f);
            //horizontal speed
            if(Input.GetKey("left shift")) //sprint
            {
                move *= sprintSpeed;
            }
            else if(Input.GetKey("left ctrl")) //crouch
            {
                move *= crouchSpeed;
            }
            else //walk
            {
                move *= speed;
            }
            if(isGrounded || airControl)
            {
                velocity.x = move.x;
                velocity.z = move.z;
            }

            //jump
            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * startGravity);
            }
            
            //less floaty jump
            if(controller.velocity.y < 0 || (controller.velocity.y > 0 && !Input.GetButton("Jump")))
            {
                gravity = startGravity * fallingGravityMultiplier;
            }
            else
            {
                gravity = startGravity;
            }
            
            //apply gravity
            velocity.y += gravity * Time.deltaTime;
            // perform movement
            controller.Move(velocity * Time.deltaTime); //only one .Move()
        }
    }
}
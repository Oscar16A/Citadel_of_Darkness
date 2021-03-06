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
    RaycastHit hit;
    public bool airControl;
    Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    [Header("Animation Queues")]
    public bool isGrounded;
    public bool walking;
    public bool freeze = false;

    //For checking to see if the footstep sounds are playing.
    private bool walkFootstepsPlaying = false;
    private bool runFootstepsPlaying = false;
    private bool landingPlayed = true; //True if the landing sound has played after jumping.

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
            Physics.SphereCast(transform.position, groundDistance, -Vector3.up, out hit, groundMask);
            bool canWalk = Vector3.Angle(hit.normal,Vector3.up) <= controller.slopeLimit;

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


                //Trigger running sounds, stop walking sounds if they are playing
                if (!runFootstepsPlaying && (velocity.x != 0 || velocity.z != 0) && isGrounded) 
                {
                    if (walkFootstepsPlaying)
                        AkSoundEngine.PostEvent("Stop_Footsteps_Walking", gameObject);
                    AkSoundEngine.PostEvent("Play_Footsteps_Running", gameObject);
                    runFootstepsPlaying = true;
                    walkFootstepsPlaying = false;
                }
                //Stop running sounds if not moving
                else if ( (velocity.x == 0 && velocity.z == 0)) 
                {
                    AkSoundEngine.PostEvent("Stop_Footsteps_Running", gameObject);
                    runFootstepsPlaying = false;
                }      
            }
            else if(Input.GetKey("left ctrl")) //crouch
            {
                move *= crouchSpeed;
            }
            else //walk
            {
                move *= speed;

                //Trigger walking sounds, stop running sounds if they are playing
                if (!walkFootstepsPlaying && (velocity.x != 0 || velocity.z != 0) && isGrounded) 
                {
                    if (runFootstepsPlaying)
                        AkSoundEngine.PostEvent("Stop_Footsteps_Running", gameObject);
                    AkSoundEngine.PostEvent("Play_Footsteps_Walking", gameObject);
                    walkFootstepsPlaying = true;
                    runFootstepsPlaying = false;
                }
                //stop walking sounds if not moving
                else if ( (velocity.x == 0 && velocity.z == 0)) 
                {
                    AkSoundEngine.PostEvent("Stop_Footsteps_Walking", gameObject);
                    walkFootstepsPlaying = false;
                }
            }
            if(isGrounded || airControl)
            {
                velocity.x = move.x;
                velocity.z = move.z;
            }

            //jump
            if(Input.GetButtonDown("Jump") && isGrounded && canWalk)
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

        //Stops footsteps from playing if the player is in the air, sets landingPlayed to false
        if (!isGrounded)
        {
            if (runFootstepsPlaying)
                AkSoundEngine.PostEvent("Stop_Footsteps_Running", gameObject);
            if (walkFootstepsPlaying)
                AkSoundEngine.PostEvent("Stop_Footsteps_Walking", gameObject);

            walkFootstepsPlaying = false;
            runFootstepsPlaying = false;

            landingPlayed = false;
        }
        else if (!landingPlayed)
        {
            AkSoundEngine.PostEvent("Play_Jump_Landing", gameObject);
            AkSoundEngine.PostEvent("Play_Jump_Landing_Cloth", gameObject);
            landingPlayed = true;
        }                
    }
}

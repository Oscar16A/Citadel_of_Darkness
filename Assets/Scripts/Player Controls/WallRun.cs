using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallRun : MonoBehaviour
{
    private enum Lean {
        None,
        Right,
        Left
    }
    private Lean stance;
    private PlayerMovement2 movement;
    private CharacterController controller;
    private LayerMask myMask;
    private bool validWall;
    private bool keepRunning;
    private bool jumpBuffer;
    private float startGravity;
    private float timeLeft;
    private float runTimer;
    private ControllerColliderHit myHit;
    public Animator Anim;
    public bool freeze = false;
    //For checking to see if the footstep sounds are playing.
    private bool wallRunPlaying = false;
    private void Start()
    {
        movement = GetComponent<PlayerMovement2>();
        controller = GetComponent<CharacterController>();
        myMask = movement.groundMask;
        startGravity = movement.gravity;
        freeze = false;
        Return2Idle();
    }

    private void Update()
    {
        //check if still on wall
        if(!freeze)
        {
            OffWallCheck();
            //OnWallCheck2();
            if((Input.GetButton("Jump") && validWall && IsFalling()) || (keepRunning && validWall))
            {
                keepRunning = true;
                Run();
                //AUDIO
                //Play Wall Run sounds if they are not currently playing.
                if (!wallRunPlaying)
                {
                    AkSoundEngine.PostEvent("Play_Footsteps_Wall_Running", gameObject);
                    wallRunPlaying = true;
                }        
            }
            else
            {
                movement.freezeY = false;
                movement.slideOff = true;
                Return2Idle();   
                //AUDIO
                //Stop wall running sounds if they are currently playing.
                if (wallRunPlaying)
                {
                    AkSoundEngine.PostEvent("Stop_Footsteps_Wall_Running", gameObject);
                    wallRunPlaying = false; 
                }         
            }
            UpdateAnim();
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        myHit = hit;
        OnWallCheck();
    }

    private bool IsFalling()
    {
        return controller.velocity.y <= 0f;
    }
    private void UpdateAnim()
    {
        switch (stance)
        {
            case Lean.None:
                Anim.SetTrigger("No Lean");
                Anim.ResetTrigger("Right Lean");
                Anim.ResetTrigger("Left Lean");
                break;
            case Lean.Right:
                Anim.ResetTrigger("No Lean");
                Anim.SetTrigger("Right Lean");
                Anim.ResetTrigger("Left Lean");
                break;
            case Lean.Left:
                Anim.ResetTrigger("No Lean");
                Anim.ResetTrigger("Right Lean");
                Anim.SetTrigger("Left Lean");
                break;
        }
    }
    private void OffWallCheck()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0f)
        {
            validWall = false;
            Return2Idle();
        }
    }
    private void OnWallCheck()
    {
        if(!movement.isGrounded && 
            (Vector3.Angle(myHit.normal, Vector3.up) > controller.slopeLimit) && 
            (Vector3.Angle(myHit.normal, Vector3.up) < 112.5f) )
            // && (Vector3.Angle(Vector3.Normalize(new Vector3(myHit.normal.x, 0f, myHit.normal.z)), transform.forward) < 157.5f) )
        {
            Debug.DrawRay(myHit.point, myHit.normal, Color.green, 1.25f);
            timeLeft = 0.1f;
            validWall = true;
        }
    }

    private void OnWallCheck2()
    {
        if(Physics.CheckSphere(transform.position, 0.75f, myMask) && !movement.isGrounded)
        {
            validWall = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.75f);
    }

    private void Run()
    {
        runTimer -= Time.deltaTime;
        movement.freezeY = true;
        movement.slideOff = false;
        // Debug.Log(Vector3.SignedAngle(myHit.normal, transform.forward, Vector3.up));
        if((Vector3.SignedAngle(myHit.normal, transform.forward, Vector3.up) > 22.5f) && (Input.GetAxisRaw("Vertical") > 0f) && (runTimer > 0f))//wall right
        {
            Vector3 direction = Vector3.Normalize(new Vector3(myHit.normal.x, 0f, myHit.normal.z));
            direction = Quaternion.Euler(0f,100f,0f) * direction;
            float yVelocity = movement.velocity.y;
            movement.velocity = direction * movement.speed;
            movement.velocity.y = yVelocity * (runTimer/2f);
            stance = Lean.Left;
        }
        else if ((Vector3.SignedAngle(myHit.normal, transform.forward, Vector3.up) < -22.5f) && (Input.GetAxisRaw("Vertical") > 0f) && (runTimer > 0f))//wall left
        {
            Vector3 direction = Vector3.Normalize(new Vector3(myHit.normal.x, 0f, myHit.normal.z));
            direction = Quaternion.Euler(0f,-100f,0f) * direction;
            float yVelocity = movement.velocity.y;
            movement.velocity = direction * movement.speed;
            movement.velocity.y = yVelocity * (runTimer/2f);
            stance = Lean.Right;
        }
        else if(Input.GetAxisRaw("Vertical") < 0f || runTimer < 0f)
        {
            keepRunning = false;
            stance = Lean.None;
            jumpBuffer = false;
        }
        else
        {
            Vector3 direction = Vector3.Normalize(new Vector3(myHit.normal.x, 0f, myHit.normal.z));
            float yVelocity = movement.velocity.y;
            movement.velocity = -direction;
            movement.velocity.y = yVelocity * (runTimer/2f);
            stance = Lean.None;
        }

        if(runTimer < 1.5f)
        {
            movement.freezeY = false;
            movement.slideOff = true;
        }

        if(Input.GetButtonUp("Jump") && jumpBuffer)
        {
            HopOff();
            Return2Idle();
        }
        jumpBuffer = true;
    }    
    private void HopOff()
    {
        //horizontal movemvent
        Vector3 direction = Vector3.Normalize(new Vector3(myHit.normal.x, 0f, myHit.normal.z));
        movement.velocity += direction * movement.sprintSpeed;
        //vertical movement
        movement.velocity.y = Mathf.Sqrt(movement.jumpHeight * -2 * startGravity);

        //clean up
        movement.freezeY = false;
        movement.slideOff = true;
        validWall = false;
    }

    private void Return2Idle()
    {
        keepRunning = false;
        stance = Lean.None;
        jumpBuffer = false;
        runTimer = 2.5f;
    }
}

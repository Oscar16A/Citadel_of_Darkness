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
    private float startGravity;
    private float timeLeft;
    private ControllerColliderHit myHit;
    private void Start()
    {
        movement = GetComponent<PlayerMovement2>();
        controller = GetComponent<CharacterController>();
        myMask = movement.groundMask;
        startGravity = movement.gravity;
        stance = Lean.None;
    }

    private void Update()
    {
        //Debug.Log(stance);
        //check if still on wall
        OffWallCheck();

        if((Input.GetAxisRaw("Vertical") > 0) && validWall)
        {
            Run();
        }
        else
        {
            movement.freezeY = false;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(hit.normal);
        myHit = hit;
        OnWallCheck();
    }
    private void OffWallCheck()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0f)
        {
            validWall = false;
            stance = Lean.None;
        }
    }
    private void OnWallCheck()
    {
        if(!movement.isGrounded && (myHit.normal.y < 0.1f && myHit.normal.y > -0.1f) && Falling())
        {
            Debug.DrawRay(myHit.point, myHit.normal, Color.red, 1.25f);
            timeLeft = 0.1f;
            validWall = true;
        }
    }
    private bool Falling()
    {
        return controller.velocity.y <= 0f;
    }
    private void Run()
    {
        movement.freezeY = true;

        if(Vector3.SignedAngle(myHit.normal, transform.forward, Vector3.up) > 0f)//wall right
        {
            Vector3 direction = Vector3.Normalize(new Vector3(myHit.normal.x, 0f, myHit.normal.z));
            direction = Quaternion.Euler(0f,100f,0f) * direction;
            movement.velocity = direction * movement.speed;
            stance = Lean.Left;
        }
        else//wall left
        {
            Vector3 direction = Vector3.Normalize(new Vector3(myHit.normal.x, 0f, myHit.normal.z));
            direction = Quaternion.Euler(0f,-100f,0f) * direction;
            movement.velocity = direction * movement.speed;
            stance = Lean.Right;
        }

        if(Input.GetButtonDown("Jump"))
        {
            HopOff();
            stance = Lean.None;
        }
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
        validWall = false;
    }
}

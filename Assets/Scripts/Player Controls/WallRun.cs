using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    private PlayerMovement2 movement;
    private CharacterController controller;
    private LayerMask myMask;
    private bool validWall;
    private float startGravity;

    private void Start()
    {
        movement = GetComponent<PlayerMovement2>();
        controller = GetComponent<CharacterController>();
        myMask = movement.groundMask;
        startGravity = movement.gravity;
    }

    private void Update()
    {
        RaycastHit hit = new RaycastHit();
        if(!movement.isGrounded)
        {
            if(CheckWall(ref hit) && Input.GetKey("w"))
            {
                //Debug.Log(hit.normal + " hit normal");
                //Debug.Log(Vector3.SignedAngle(hit.normal, transform.forward, Vector3.up) + " compare angle forward");//negative left, positive right
                Run(ref hit);
            }
            else
            {
                movement.freezeY = false;
            }
        }
        else
        {
            movement.freezeY = false;
        }
    }

    private bool CheckWall(ref RaycastHit hit)
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, 2f, myMask)) //facing a wall
        {
            if(Falling())
            {
                //Debug.Log("valid wall");
                return true;
            }
        }
        //Debug.Log("not valid wall");
        return false;
    }
    private void Run(ref RaycastHit hit)
    {
        movement.freezeY = true;
        if(Input.GetKeyDown("space"))
        {
            HopOff(ref hit);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }

    private bool Falling()
    {
        return controller.velocity.y <= 0f;
    }
    private void HopOff(ref RaycastHit hit)
    {
        movement.velocity = Vector3.zero;
        

        if(Vector3.SignedAngle(hit.normal, transform.forward, Vector3.up) > 0f)//wall right, move left
        {
            movement.velocity += -transform.right * movement.sprintSpeed;

        }
        else//wall left, move rigt
        {
            movement.velocity += transform.right * movement.sprintSpeed;
        }

        movement.velocity.y = Mathf.Sqrt(movement.jumpHeight * -2 * startGravity);//add vertical movement
        movement.freezeY = false;
    }
}

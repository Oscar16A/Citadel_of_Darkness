using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    CharacterController controller;
    public float speed = 6f;
    public float sprintSpeed = 9f;
    public float crouchSpeed = 3f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public float fallingGravityMultiplier = 2f;
    private float startGravity;
    public Vector3 velocity;
    RaycastHit hit;
    public bool airControl;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    [Header("Animation Queues")]
    public bool isGrounded;
    public bool walking;
    public bool freeze = false;
    public bool freezeY = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
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
            if(Input.GetButton("Sprint")) //sprint
            {
                move *= sprintSpeed;
            }
            else if(Input.GetButton("Crouch")) //crouch
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
            //don't apply gravity if freezeY true
            if(freezeY)
            {
                velocity.y = 0f;
            }
            // perform movement
            controller.Move(velocity * Time.deltaTime); //only one .Move()
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}

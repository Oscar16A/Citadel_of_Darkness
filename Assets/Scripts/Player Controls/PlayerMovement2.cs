using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2 : MonoBehaviour
{
    CharacterController controller;
    public float speed = 6f;
    public float sprintSpeed = 9f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    public float fallingGravityMultiplier = 2f;
    private float startGravity;
    public Vector3 velocity;
    RaycastHit hit;
    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;
    [Header("Animation Queues")]
    public bool isGrounded;
    public bool freeze = false;
    public bool freezeY = false;
    public bool slideOff = true;
    //For checking to see if the footstep sounds are playing.
    private bool walkFootstepsPlaying = false;
    private bool runFootstepsPlaying = false;
    private bool landingPlayed = true; //True if the landing sound has played after jumping.
    void Start()
    {
        controller = GetComponent<CharacterController>();
        startGravity = gravity;
        slideOff = true;
    }
    void Update()
    {
        if(!freeze)
        {
            
            if(Physics.SphereCast(transform.position, groundDistance, -Vector3.up, out hit, Vector3.Distance(transform.position, groundCheck.position),  groundMask) && 
               Vector3.Angle(hit.normal,Vector3.up) < controller.slopeLimit)
            {
                isGrounded = true;
                Debug.DrawRay(hit.point, hit.normal, Color.blue, 1f);
            }
            else
            {
                isGrounded = false;
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1f);
            }

            if(isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            //horizontal movement
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 moveDir = transform.right * x + transform.forward * z;
            moveDir = Vector3.ClampMagnitude(moveDir, 1f);

            //horizontal speed
            if(!Input.GetButton("Sprint")) //sprint
            {
                moveDir *= sprintSpeed;

                //AUDIO
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
            else //walk
            {
                moveDir *= speed;

                //AUDIO
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


            if(isGrounded)
            {
                velocity = moveDir;
            }
            else if(slideOff && velocity.y < 0f)
            {
                if(Physics.SphereCast(transform.position, groundDistance, -Vector3.up, out hit, Vector3.Distance(transform.position, groundCheck.position)))
                {
                    Vector3 slideDir = hit.normal * 2f * Mathf.Cos(Mathf.Deg2Rad * Vector3.Angle(hit.normal, Vector3.up));

                    velocity = slideDir + (-Vector3.up * 2f);
                }
            }

            //jump
            if(Input.GetButtonDown("Jump"))
            {
                if(isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2 * startGravity);
                }
                // else if(slideOff && velocity.y < 0f)
                // {
                //     float jumpForce = Mathf.Sqrt(jumpHeight * -2 * startGravity);
                //     if(Physics.SphereCast(transform.position, groundDistance, -Vector3.up, out hit, Vector3.Distance(transform.position, groundCheck.position)))
                //     {
                //         velocity = hit.normal * jumpForce;
                //     }
                // } //won't work with current input (using jump)
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}

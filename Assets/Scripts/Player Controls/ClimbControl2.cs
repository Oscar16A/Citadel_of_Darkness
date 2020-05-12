using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbControl2 : MonoBehaviour
{
    private PlayerMovement2 movement;
    private CharacterController controller;
    private WallRun wallRun;
    private bool validClimb;
    private LayerMask myMask;
    public float animLength = 0.34f; //in seconds
    public Animator anim;
    private bool freeze;
    Vector3 fS;
    Vector3 uS;
    private void Start()
    {
        movement = GetComponent<PlayerMovement2>();
        controller = GetComponent<CharacterController>();
        wallRun = GetComponent<WallRun>();
        myMask = movement.groundMask;
    }
    private void Update()
    {
        // RaycastHit upSphere = new RaycastHit();
        // if(Physics.SphereCast(transform.position, 0.5f, Vector3.up, out upSphere, 2.5f, myMask))
        // {
        //     uS = transform.position + Vector3.up * upSphere.distance;
        // }
        // else
        // {
        //     uS = transform.position + Vector3.up * 2.5f;
        // }

        // RaycastHit forwardSphere = new RaycastHit();
        // if(Physics.SphereCast(transform.position + Vector3.up * 2.5f, 0.5f, transform.forward, out forwardSphere, 1f, myMask))
        // {
        //     fS = (transform.position + Vector3.up * 2.5f) + transform.forward * forwardSphere.distance;
        // }
        // else
        // {
        //     fS = (transform.position + Vector3.up * 2.5f) + transform.forward * 1f;
        // }

        if(!movement.isGrounded)
        {
            RaycastHit hit = new RaycastHit();
            
            validClimb = CheckClimb(ref hit) && SpaceCheck();
            // Debug.Log(hit.normal);
            // if(Input.GetAxisRaw("Vertical") > 0f && validClimb)
            if(Input.GetButton("Jump") && validClimb)
            {
                if(!freeze)
                {
                    ClimbLedge(ref hit);
                }
            }
        }
    }

    private bool SpaceCheck()
    {
        RaycastHit throwAway;
        return (!(Physics.SphereCast(transform.position, 0.5f, Vector3.up, out throwAway, 2.5f, myMask)) && //space above
                !(Physics.SphereCast(transform.position + Vector3.up * 2.5f, 0.5f, transform.forward, out throwAway, 1f, myMask)) ); //space above and forward
    }

    private bool CheckClimb(ref RaycastHit hit)
    {
        if(Physics.SphereCast(transform.position + transform.forward + new Vector3(0f,2.5f,0f), 0.5f, -Vector3.up, out hit, 2f, myMask))
        {
            if(Vector3.Angle(hit.normal, Vector3.up) <= controller.slopeLimit)
            {
                RaycastHit throwAway;
                if(!Physics.SphereCast(hit.point, 0.6f, Vector3.up, out throwAway, 2f, myMask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void ClimbLedge(ref RaycastHit hit)
    {
        movement.freeze = true;
        wallRun.freeze = true;
        freeze = true;
        controller.Move(transform.forward + new Vector3(0f,hit.point.y-transform.position.y + 1f,0f));
        ClimbAnim();
        UnfreezeDelay(animLength);
    }

    void ClimbAnim()
    {
        anim.SetTrigger("Climb");
        anim.ResetTrigger("No Lean");
        anim.ResetTrigger("Left Lean");
        anim.ResetTrigger("Right Lean");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + transform.forward + new Vector3(0f,2f,0f), -transform.up*2.5f);
        // Gizmos.color = Color.blue;
        // Gizmos.DrawWireSphere(uS, 0.5f);//up
        // Gizmos.DrawWireSphere(fS, 0.5f);//forward
    }

    private void UnfreezeDelay(float time)
    {
        IEnumerator delay = WaitSeconds(time);
        StartCoroutine(delay);
    }

    IEnumerator WaitSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        movement.velocity = new Vector3(0f,-2f,0f);
        movement.freeze = false;
        wallRun.freeze = false;
        freeze = false;
    }
}

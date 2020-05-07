using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbControl2 : MonoBehaviour
{
    private PlayerMovement2 movement;
    private CharacterController controller;
    private bool validClimb;
    private LayerMask myMask;
    public float animLength = 0.34f; //in seconds
    public Animator anim;
    public float angleTolerance = 0f; // how far the ledge can be tillted and still be climbed
    private bool freeze;
    Vector3 fS;
    Vector3 uS;
    private void Start()
    {
        movement = GetComponent<PlayerMovement2>();
        controller = GetComponent<CharacterController>();
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
            if((Input.GetAxisRaw("Vertical") > 0) && validClimb)
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
        return (!(Physics.SphereCast(transform.position, 0.5f, Vector3.up, out throwAway, 2.5f, myMask)) && 
                !(Physics.SphereCast(transform.position + Vector3.up * 2.5f, 0.5f, transform.forward, out throwAway, 1f, myMask)));
    }

    private bool CheckClimb(ref RaycastHit hit)
    {
        if(Physics.Raycast(transform.position + transform.forward + new Vector3(0f,2f,0f), -transform.up, out hit, 2.5f, myMask))
        {
            if(Vector3.Angle(hit.normal, Vector3.up) <= angleTolerance)
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
        freeze = true;
        controller.Move(transform.forward + new Vector3(0f,hit.point.y-transform.position.y + 1f,0f));
        anim.SetTrigger("Climb");
        UnfreezeDelay(animLength);
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
        freeze = false;
    }
}

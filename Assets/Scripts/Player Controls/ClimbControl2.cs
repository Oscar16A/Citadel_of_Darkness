using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbControl2 : MonoBehaviour
{
    private PlayerMovement2 movement;
    private CharacterController controller;
    private bool validClimb;
    private LayerMask myMask;
    public Animator anim;
    public float angleTolerance = 0f; // how far the ledge can be tillted and still be climbed
    private void Start()
    {
        movement = GetComponent<PlayerMovement2>();
        controller = GetComponent<CharacterController>();
        myMask = movement.groundMask;
    }
    private void Update()
    {
        if(!movement.isGrounded)
        {
            RaycastHit hit = new RaycastHit();
            validClimb = CheckClimb(ref hit);
            if(Input.GetKey("w") && validClimb)
            {
                ClimbLedge(ref hit);
            }
        }
    }

    private bool CheckClimb(ref RaycastHit hit)
    {
        if(Physics.Raycast(transform.position + transform.forward + new Vector3(0f,2f,0f), -transform.up, out hit, 2.5f, myMask))
        {
            if(Vector3.Angle(hit.normal, Vector3.up) <= angleTolerance)
            {
                return true;
            }
        }

        return false;
    }

    private void ClimbLedge(ref RaycastHit hit)
    {
        movement.freeze = true;
        controller.Move(transform.forward + new Vector3(0f,hit.point.y-transform.position.y + 1f,0f));
        anim.SetTrigger("Climb");
        UnfreezeDelay(0.34f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + transform.forward + new Vector3(0f,2f,0f), -transform.up*2.5f);
    }

    private void UnfreezeDelay(float time)
    {
        IEnumerator delay = WaitSeconds(time);
        StartCoroutine(delay);
    }

    IEnumerator WaitSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        movement.freeze = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbControl : MonoBehaviour
{
    private PlayerMovement movement;
    private CharacterController controller;
    private bool validClimb;
    private bool oneClimb;
    public LayerMask myMask;
    public Animator anim;
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if(!movement.isGrounded)
        {
            RaycastHit hit = new RaycastHit();
            validClimb = CheckClimb(ref hit);
            if(Input.GetKeyDown("space") && validClimb && oneClimb)
            {
                oneClimb = false;
                ClimbLedge(ref hit);
            }
        }
        else
        {
            oneClimb = true;
        }
    }

    private bool CheckClimb(ref RaycastHit hit)
    {
        if(Physics.Raycast(transform.position + transform.forward + new Vector3(0f,2f,0f), -transform.up, out hit, 2f, myMask))
        {
            if(hit.normal == Vector3.up)
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
        UnfreezeDelay(0.2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + transform.forward + new Vector3(0f,2f,0f), -transform.up*2f);
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

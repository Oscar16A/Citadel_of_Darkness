using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractControl : MonoBehaviour
{
    public Transform myCamera;
    public LayerMask myMask;
    public float interactDistance = 2f;
    public GameObject interactText;
    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(myCamera.position, myCamera.forward, out hit, interactDistance, myMask);
        Transform hitTransform = hit.transform;
        if(interactText != null)
        {
            interactText.SetActive(false);
        }
        if(hitTransform != null)
        {
            Interactable hitInteract = hitTransform.GetComponent<Interactable>();
            if(hitInteract != null)
            {
                if(interactText != null)
                {
                    interactText.SetActive(true);
                }
                if(Input.GetKeyDown("e"))
                {
                    hitInteract.HitTrigger();
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(myCamera.position, myCamera.forward * interactDistance);
    }
}

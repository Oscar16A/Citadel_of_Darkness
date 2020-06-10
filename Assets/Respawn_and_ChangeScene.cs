using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_and_ChangeScene : MonoBehaviour
{
    public Vector3 checkpointPos;
    private PlayerMovement2 movement2;
    private bool teleportBuffer;

    private void Start()
    {
        checkpointPos = this.transform.position;
        Debug.Log(checkpointPos);
        movement2 = GetComponent<PlayerMovement2>();
    }

    private void LateUpdate()
    {
        if(teleportBuffer)
        {
            Debug.Log("Moved Position:");
            Debug.Log(this.transform.position);
            this.transform.position = checkpointPos;
            teleportBuffer = false;
            movement2.freeze = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "checkpoint")
        {
            Debug.Log("checkpoint!");
            checkpointPos = this.transform.position;
            other.gameObject.SetActive(false);
            Debug.Log(checkpointPos);
        }

        else if (other.tag == "killVolume")
        {
            movement2.freeze = true;
            movement2.velocity = Vector3.zero;
            teleportBuffer = true;
            
        }
    }
}

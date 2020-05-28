using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_and_ChangeScene : MonoBehaviour
{
    public Vector3 checkpointPos;
    public GameObject player;

    private PlayerMovement2 movement2;
    private bool teleportBuffer;

    private void Start()
    {
        checkpointPos = this.transform.position;
        Debug.Log(checkpointPos);
        movement2 = GetComponent<PlayerMovement2>();
    }

    private void Update()
    {
        if(teleportBuffer)
        {
            teleportBuffer = false;
            movement2.freeze = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OOF");
        if(other.tag == "checkpoint")
        {
            Debug.Log("checkpoint!");
            checkpointPos = this.transform.position;
            other.gameObject.SetActive(false);
            Debug.Log(checkpointPos);
        }

        else if (other.tag == "killVolume")
        {
            //Debug.Log("doubleOOF");
            //Debug.Log("stored position:");
            Debug.Log(checkpointPos);
            movement2.freeze = true;
            this.transform.position = checkpointPos;
            movement2.velocity = Vector3.zero;
            teleportBuffer = true;
            Debug.Log("Moved Position:");
            Debug.Log(player.transform.position);
        }
    }
}

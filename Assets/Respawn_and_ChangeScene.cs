using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_and_ChangeScene : MonoBehaviour
{
    public Vector3 checkpointPos;
    public GameObject player;

    private void Start()
    {
        checkpointPos = this.transform.position;
        Debug.Log(checkpointPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OOF");
        if(other.tag == "checkpoint")
        {
            Debug.Log("checkpoint!");
            checkpointPos = other.transform.position;
            Debug.Log(checkpointPos);
        }

        else if (other.tag == "killVolume")
        {
            Debug.Log("doubleOOF");
            Debug.Log("stored position:");
            Debug.Log(checkpointPos);
            this.transform.position = checkpointPos;
            Debug.Log("Moved Position:");
            Debug.Log(player.transform.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
            SceneManager.LoadScene("MainMenu");
    }
}

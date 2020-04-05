using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    public GameObject flashLight;

    private void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            flashLight.SetActive(!flashLight.activeSelf);
        }
    }
}

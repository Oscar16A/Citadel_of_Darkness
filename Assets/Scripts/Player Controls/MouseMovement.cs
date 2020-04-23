using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement: MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float controllerSensitivity = 200f;
    public Transform playerBody;
    private Transform playerCamera;
    private float xRotation = 0f;
    void Start()
    {
        playerCamera = transform.Find("Camera");
        Cursor.lockState = CursorLockMode.Locked;
		mouseSensitivity = StatsData.mouseSensitivity;
		//print(StatsData.mouseSensitivity);
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        mouseX += Input.GetAxis("Right_Horizontal") * controllerSensitivity * Time.deltaTime;
        mouseY += Input.GetAxis("Right_Vertical") * controllerSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // https://www.sharpcoderblog.com/blog/unity-3d-fps-controller

    public float speed;
    public float gravity;
    public Camera camera;
    public float sens;
    public float lookXLimit = 45.0f;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    CharacterController controller;


    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * sens;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sens, 0);
        }
    }
}

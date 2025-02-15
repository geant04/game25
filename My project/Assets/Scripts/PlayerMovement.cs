using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform feet;
    public float jumpSpeed, moveSpeed;
    public bool isMoving;
    public float gravity;
    private float moveFB, moveLR;

    // Start is called before the first frame update
    void Start()
    {
        moveFB = 0; moveLR = 0;
    }

    // Update is called once per frame
    void Update()
    {
        moveFB = Input.GetAxisRaw("Vertical") * moveSpeed;
        moveLR = Input.GetAxisRaw("Horizontal") * moveSpeed;
        isMoving = moveFB != 0 || moveLR != 0;

        bool grounded = isGrounded();

        if (!grounded) {
            gravity += (Physics.gravity.y) * Time.deltaTime;
        } else {
            gravity = 0f;
        }

        if (grounded) {
            if (Input.GetKey(KeyCode.Space)) {
                gravity += jumpSpeed;
            }
        }
    }

    void FixedUpdate() {
        
        Vector3 movement = (Vector3.up * gravity + transform.right * moveLR + moveFB * transform.forward);
        transform.position += (Time.fixedDeltaTime * moveSpeed * movement);
    }

    bool isGrounded() {
        bool b = Physics.Raycast(feet.position, -Vector3.up, 0.25f);
        Debug.Log(b);
        return b;
    }
}

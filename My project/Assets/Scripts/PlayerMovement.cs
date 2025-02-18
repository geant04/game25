using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public int health;
    public Rigidbody myRB;
    public Transform feet;
    public float jumpSpeed, moveSpeed;
    public float gravity;
    private float moveFB, moveLR;
    private bool isDead;
    
    public GameObject camera;
    public float camTilt, trigInput;
    private float cameraY, maxTilt, maxRaise;


    [SerializeField] private TextMeshProUGUI title;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        health = 2;
        moveFB = 0; moveLR = 0;
        cameraY = camera.transform.position.y;
        maxTilt = 3;
        maxRaise = 0.03f;
    }

    // Update is called once per frame
    void Update()
    {
        moveFB = Input.GetAxisRaw("Vertical") * moveSpeed;
        moveLR = Input.GetAxisRaw("Horizontal") * moveSpeed;

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

        //camera tilt
        if (moveLR > 0.1f){
            camTilt = Mathf.Min(camTilt + 15 * Time.deltaTime, maxTilt);
        } else if (moveLR < -0.1f) {
            camTilt = Mathf.Max(camTilt - 15 * Time.deltaTime, -maxTilt);
        } else if (camTilt > 0) {
            camTilt = Mathf.Max(camTilt - 15 * Time.deltaTime, 0);
        } else if (camTilt < 0) {
            camTilt = Mathf.Min(camTilt + 15 * Time.deltaTime, 0);
        }
        camera.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x - 0.000001f, camera.transform.eulerAngles.y - 0.00000001f, -camTilt);

        //head bobbing
        if (grounded && (moveFB != 0 || moveLR != 0 || trigInput > 0.1f)) {
            trigInput += 9 * Time.deltaTime;
            if (trigInput > 2 * 3.141592f) trigInput = 0.03f;
            camera.transform.position = new Vector3(camera.transform.position.x + 0.000001f, cameraY - maxRaise * Mathf.Cos(trigInput), camera.transform.position.z - 0.00000001f);
        }

        if (title)
        {
            title.SetText($"{health}");
        }
    }

    void FixedUpdate() {
        
        Vector3 movement = (Vector3.up * gravity + transform.right * moveLR + moveFB * transform.forward);
        transform.position += (Time.fixedDeltaTime * moveSpeed * movement);
    }

    bool isGrounded() {
        bool b = Physics.Raycast(feet.position, -Vector3.up, 0.36f);
        return b;
    }

    public void TakeDamage(int amt) {
        health = Mathf.Max(0, health - amt);
        Debug.Log(" ouch i hurt, hp: " + health);
        if (health <= 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        if (isDead) return;

        Debug.Log("God I am dead");
        isDead = true;
        Destroy(gameObject);
    }
    
}

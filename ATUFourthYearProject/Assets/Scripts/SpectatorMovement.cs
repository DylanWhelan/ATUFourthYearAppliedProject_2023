using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    private float horizontalMovement;
    private float verticalMovement;

    public TraitsDisplay traitsDisplay;

    public float speed;

    private Rigidbody rb;

    private float speedMultiplier;
    private float rotX;
    private float rotY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        // This will detect forward and backward movement
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        // This will detect sideways movement
        verticalMovement = Input.GetAxisRaw("Vertical");

        float y = 0;

        speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 3.0f : 1.0f; 

        if(Input.GetKey(KeyCode.E)) y = 1;
        else if (Input.GetKey(KeyCode.Q)) y = -1;

        // Calculate the direction to move the player
        Vector3 movementDirection = transform.forward * verticalMovement + transform.right * horizontalMovement + transform.up * y;
        transform.position += movementDirection * speed * speedMultiplier * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            rotX += Input.GetAxis("Mouse X") * 1;
            rotY += Input.GetAxis("Mouse Y") * 1;

            rotY = Mathf.Clamp(rotY, -45, 45);

            transform.rotation = Quaternion.Euler(-rotY, rotX, 0);
        }
        

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit target;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out target, 100f))
            {
                if (target.transform.name.Contains("Slime"))
                {
                    traitsDisplay.UpdateStoredSlime(target.transform.GetComponent<Slime>());
                }
                else
                {
                    traitsDisplay.UpdateStoredSlime();
                }
                
            }
            else
            {
                   traitsDisplay.UpdateStoredSlime();
            }
        }
    }
}

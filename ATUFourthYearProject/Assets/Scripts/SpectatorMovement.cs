using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    private float horizontalMovement;
    private float verticalMovement;

    // A field editable from inside Unity with a default value of 5
    public float speed = 5.0f;

    // How much will the player slide on the ground
    // The lower the value, the greater distance the user will slide
    public float drag;

    private Rigidbody rb;

    private float rotX;
    private float rotY;

    // Start is called before the first frame update
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

        if(Input.GetKey(KeyCode.E)) y = 1;
        else if (Input.GetKey(KeyCode.Q)) y = -1;

        // Calculate the direction to move the player
        Vector3 movementDirection = transform.forward * verticalMovement + transform.right * horizontalMovement + transform.up * y;
        transform.position += movementDirection * speed * Time.deltaTime;

        rotX += Input.GetAxis("Mouse X") * 1;
        rotY += Input.GetAxis("Mouse Y") * 1;

        rotY = Mathf.Clamp(rotY, -45, 45);

        transform.rotation = Quaternion.Euler(-rotY, rotX, 0);
    }
}

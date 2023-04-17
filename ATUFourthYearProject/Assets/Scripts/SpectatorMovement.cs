using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    private float _horizontalMovement;
    private float _verticalMovement;

    public TraitsDisplay traitsDisplay;

    public float _speed;

    private Rigidbody _rb;

    private float _speedMultiplier;
    private float _rotX;
    private float _rotY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        // This will detect forward and backward movement
        _horizontalMovement = Input.GetAxisRaw("Horizontal");

        // This will detect sideways movement
        _verticalMovement = Input.GetAxisRaw("Vertical");

        float y = 0;

        _speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 3.0f : 1.0f; 

        if(Input.GetKey(KeyCode.E)) y = 1;
        else if (Input.GetKey(KeyCode.Q)) y = -1;

        // Calculate the direction to move the player
        Vector3 movementDirection = transform.forward * _verticalMovement + transform.right * _horizontalMovement + transform.up * y;
        transform.position += movementDirection * _speed * _speedMultiplier * Time.deltaTime;

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
            _rotX += Input.GetAxis("Mouse X") * 1;
            _rotY += Input.GetAxis("Mouse Y") * 1;

            _rotY = Mathf.Clamp(_rotY, -45, 45);

            transform.rotation = Quaternion.Euler(-_rotY, _rotX, 0);
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

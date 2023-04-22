using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    // floats to store horizontal and vertical movement values
    private float _horizontalMovement;
    private float _verticalMovement;


    // instance of traitsdisplay class
    public TraitsDisplay traitsDisplay;

    // float modifies spectators speed
    public float _speed;

    private Rigidbody _rb;

    // used to increase speed when shift is pressed
    private float _speedMultiplier;

    // specifies rotation on both axes
    private float _rotX;
    private float _rotY;

    void Start()
    {
        // The cursor's lockstate is set to lockewd
        Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Escape toggles the lockstate of cursor
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

        // If cursor is locked, spectator is in movement state
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // This will detect forward and backward movement
            _horizontalMovement = Input.GetAxisRaw("Horizontal");

            // This will detect sideways movement
            _verticalMovement = Input.GetAxisRaw("Vertical");

            float y = 0;

            // if shift is pressed triple spectators speed
            _speedMultiplier = Input.GetKey(KeyCode.LeftShift) ? 3.0f : 1.0f;

            // if E is pressed, spectators moves upwards, if Q is pressed, spectator moves downwards
            if (Input.GetKey(KeyCode.E)) y = 1;
            else if (Input.GetKey(KeyCode.Q)) y = -1;

            // Calculate the direction to move the player
            Vector3 movementDirection = transform.forward * _verticalMovement + transform.right * _horizontalMovement + transform.up * y;
            transform.position += movementDirection * _speed * _speedMultiplier * Time.deltaTime;

            _rotX += Input.GetAxis("Mouse X") * 1;
            _rotY += Input.GetAxis("Mouse Y") * 1;

            _rotY = Mathf.Clamp(_rotY, -60, 60);

            transform.rotation = Quaternion.Euler(-_rotY, _rotX, 0);

            // If mouse click
            if (Input.GetMouseButtonDown(0))
            {
                // Raycast to see if a slime is clicked on
                RaycastHit target;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out target, 100f))
                {
                    // if raycast hits slime, called updatedStoredSlime and pass in the hit slime
                    if (target.transform.name.Contains("Slime"))
                    {
                        traitsDisplay.UpdateStoredSlime(target.transform.GetComponent<Slime>());
                    }
                    else
                    {
                        traitsDisplay.UpdateStoredSlime();
                    }

                }
                // if raycast fails, call method with no parameters to wipe away stored slime
                else
                {
                    traitsDisplay.UpdateStoredSlime();
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Inspector fields
    [Header("Mechanics")]
    [SerializeField] float _moveSpeed = 12.0f;     // Move speed in x and z
    [SerializeField] float _sprintMultiplier = 2.0f;  
    [SerializeField] float _jumpHeight = 3.0f;
    [SerializeField] float _gravity = -19.62f;     // Simulated gravity at 2*g
    [SerializeField] float _groundDistance = 0.4f; // Sphere cast radius

    [Header("Components")]
    [SerializeField] CharacterController _charController;
    [SerializeField] Transform _groundCheck;
    [SerializeField] LayerMask _groundMask;

    // Class variables
    Vector3 _velocity;      // velocity to holds physics based forces
    bool _grounded;
    bool _sprinting;

    void Update()
    {
        // Check if grounded this frame using Physics sphere cast
        _grounded = Physics.CheckSphere(_groundCheck.position, _groundDistance,
            _groundMask);
        // Capture keyboard input on shift, sprinting if held down this frame
        _sprinting = Input.GetKey(KeyCode.LeftShift);

        // Make sure gravity isn't accumulating on the velocity while grounded
        if(_grounded && _velocity.y < 0)
        {
            _velocity.y = -2.0f;
        }

        // Capture keyboard input on wasd & up/down/left/right
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Create movement vector
        Vector3 move = transform.right * x + transform.forward * z;
        // If sprinting, apply sprint to forward velocity
        if(_sprinting && move.z > 0)
        {
            move.z *= _sprintMultiplier;
        }

        // Apply input movement to character controller
        _charController.Move(_moveSpeed * Time.deltaTime * move);

        // Capture keyboard input on spacebar
        // Apply jump to y velocity
        if(Input.GetButtonDown("Jump") && _grounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }

        // Apply gravity
        _velocity.y += _gravity * Time.deltaTime;

        // Apply physics force movement to character controller
        _charController.Move(_velocity * Time.deltaTime);
    }
}

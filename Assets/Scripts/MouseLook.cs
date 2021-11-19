using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Inspector fields
    [Header("Mechanics")]
    [SerializeField] float _mouseSens = 200.0f;

    [Header("Components")]
    [SerializeField] Transform _playerTransform;

    // Class variables
    float _xRotation = 0.0f;

    void Update()
    {
        // Capture mouse input on x and y axes
        float mouseX = Input.GetAxis("Mouse X") * _mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSens * Time.deltaTime;

        // Rotate the player in the y direction clamped between +/- z
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // Rotate the player in the x direction
        _playerTransform.Rotate(Vector3.up * mouseX);
    }
}

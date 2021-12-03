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
    [SerializeField] Transform _firstPersonPerspective;
    [SerializeField] Transform _thirdPersonPerspective;

    // Class variables
    float _xRotation = 0.0f;
    bool _firstPerson = true;
    float _transitionDuration = 0.25f;

    void Start()
    {
        transform.position = _firstPersonPerspective.transform.position;
    }

    void Update()
    {
        // Capture mouse input on x and y axes
        float mouseX = Input.GetAxis("Mouse X") * _mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSens * Time.deltaTime;


        // Rotate the player in the y direction clamped between +/- z axis
        _xRotation -= mouseY;
        if(_firstPerson)
        {
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        }
        else
        {
            _xRotation = Mathf.Clamp(_xRotation, -50f, 50f);
        }
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // Rotate the player in the x direction
        _playerTransform.Rotate(Vector3.up * mouseX);
    }

    public void TogglePerspective()
    {
        if(_firstPerson)
        {
            _firstPerson = false;
            StartCoroutine(CameraTransition(_thirdPersonPerspective));
        }
        else
        {
            _firstPerson = true;
            StartCoroutine(CameraTransition(_firstPersonPerspective));
        }
    }

    IEnumerator CameraTransition(Transform target)
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        while(t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / _transitionDuration);
            transform.position = Vector3.Lerp(startingPos, target.position, t);
            yield return 0;
        }
    }
}

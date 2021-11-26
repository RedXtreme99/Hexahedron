using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    // Inspector fields to control raycast parameters
    [SerializeField] float _rayDistance = 50.0f;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] Transform _shadowCastSpawn;
    [SerializeField] LayerMask _geometryLayer;

    // Class variables
    RaycastHit _hitInfo;        // raycast hit info to capture from raycasts

    void Update()
    {
        // Capture left mouse click to shoot raycast
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Determine if standing in shadow for mote mechanic
        bool standingInShadow = ShadowCheck();

        // Perform shoot raycast from center of screen
        Transform center = Camera.main.transform;
        if(standingInShadow) Debug.Log("SHADOW"); else Debug.Log("LIGHT");
        if(Physics.Raycast(center.position, center.forward, out _hitInfo,
            _rayDistance, _enemyLayer))
        {

        }
    }

    bool ShadowCheck()
    {
        // Check to see if standing in a shadow by checking if there is
        // geometry in the direction opposite the light
        Vector3 lightDirection =
            LevelController.Instance._levelLight.transform.forward;
        return Physics.Raycast(_shadowCastSpawn.position, -lightDirection,
            _rayDistance, _geometryLayer);
    }
}

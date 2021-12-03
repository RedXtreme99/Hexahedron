using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    // Inspector fields to control raycast parameters
    [Header("Mechanics")]
    [SerializeField] float _rayDistance = 50.0f;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] Transform _shadowCastSpawn;
    [SerializeField] LayerMask _geometryLayer;

    [Header("Components")]
    [SerializeField] Texture2D _crosshair;
    [SerializeField] AudioClip _shootSound;

    // Class variables
    RaycastHit _hitInfo;        // raycast hit info to capture from raycasts
    bool _disabled = false;

    void Update()
    {
        // Capture left mouse click to shoot raycast
        if(Input.GetKeyDown(KeyCode.Mouse0) && !_disabled)
        {
            Shoot();
        }
    }

    void OnGUI()
    {
        float xMin = Screen.width / 2 - _crosshair.width / 2;
        float yMin = Screen.height / 2 - _crosshair.height / 2;
        if(!LevelController.Instance._paused && !_disabled)
        {
            GUI.DrawTexture(new Rect(xMin, yMin, _crosshair.width,
                _crosshair.height), _crosshair);
        }
    }

    void Shoot()
    {
        // Determine if standing in shadow for mote mechanic
        bool standingInShadow = ShadowCheck();

        // Perform shoot raycast from center of screen
        Transform center = Camera.main.transform;
        AudioManager.Instance.PlaySound(_shootSound);
        if(Physics.Raycast(center.position, center.forward, out _hitInfo,
            _rayDistance, _enemyLayer))
        {
            if(_hitInfo.transform.tag == "Enemy")
            {
                Enemy enemy = _hitInfo.transform.GetComponent<Enemy>();
                if(enemy != null)
                {
                    enemy.Kill(standingInShadow);
                }
            }
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

    public void SetDisabled(bool disable)
    {
        _disabled = disable;
    }
}

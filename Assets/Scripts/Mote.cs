using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mote : MonoBehaviour
{
    // Components
    [SerializeField] Material _lightMaterial;
    [SerializeField] Material _darkMaterial;

    // Class Variables
    MeshRenderer _renderer;
    bool _dark = false;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        SetDark(true);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if(inventory != null)
        {
            Destroy(gameObject);
        }
    }

    public void SetDark(bool dark)
    {
        _dark = dark;
        if(dark)
        {
            _renderer.material = _darkMaterial;
        }
        else
        {
            _renderer.material = _lightMaterial;
        }
    }
}

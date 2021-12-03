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
    public bool _dark = false;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        SetDark(_dark);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if(inventory != null)
        {
            if(!inventory._fullMotes)
            {
                inventory.AddMote(_dark);
                Destroy(gameObject);
            }
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

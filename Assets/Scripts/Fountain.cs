using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : MonoBehaviour
{
    // Components
    [SerializeField] Material _lightMaterial;
    [SerializeField] Material _darkMaterial;

    // Class variables
    bool _dark;
    bool _active;
    LevelController.Rotation rotation;
    MeshRenderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public void SetActive(bool active)
    {
        _active = active;
        if(active)
        {
            int randInt = Random.Range(0, 2);
            if(randInt == 0)
            {
                SetDark(true);
            }
            else
            {
                SetDark(false);
            }
            _renderer.enabled = true;
            if(tag == "Front")
            {
                rotation = LevelController.Rotation.Forward;
            }
            if(tag == "Left")
            {
                rotation = LevelController.Rotation.Counterclockwise;
            }
            if(tag == "Right")
            {
                rotation = LevelController.Rotation.Clockwise;
            }
            if(tag == "Back")
            {
                rotation = LevelController.Rotation.Backward;
            }
        }
        else
        {
            _renderer.enabled = false;
        }
    }

    void SetDark(bool dark)
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

    public void Cleanse(bool incomingShadow)
    {
        if(incomingShadow == _dark)
        {
            LevelController.Instance.SetRotation(rotation);
        }
    }
}

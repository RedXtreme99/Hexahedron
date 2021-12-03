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
            else if(tag == "Left")
            {
                rotation = LevelController.Rotation.Counterclockwise;
            }
            else if(tag == "Right")
            {
                rotation = LevelController.Rotation.Clockwise;
            }
            else if(tag == "Back")
            {
                rotation = LevelController.Rotation.Backward;
            }
            else
            {
                Debug.Log("Rotation not set for: " + name);
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
            Debug.Log(rotation.ToString());
            LevelController.Instance.SetRotation(rotation);
        }
    }
}

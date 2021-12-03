using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fountain : MonoBehaviour
{
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

    public void Cleanse(bool incomingShadow)
    {
        if(incomingShadow == _dark)
        {
            LevelController.Instance.SetRotation(rotation);
        }
    }
}

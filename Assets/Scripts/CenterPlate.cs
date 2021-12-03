using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPlate : MonoBehaviour
{
    MeshRenderer _renderer;
    bool _containsTarget;
    bool _active;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(_active)
            {
                LevelController.Instance.StartRotation();
            }
        }
    }

    public void SetGoalFace(bool goal)
    {
        _containsTarget = goal;
    }

    public void SetActive(bool active)
    {
        _active = active;
        SetGlowing(active);
    }

    void SetGlowing(bool glow)
    {
        if(glow)
        {
            _renderer.enabled = true;
        }
        else
        {
            _renderer.enabled = false;
        }
    }
}

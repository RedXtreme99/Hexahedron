using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool _dropsMotes = false;

    private void Awake()
    {
        float random = Random.Range(0, 2);
        if(random == 1)
        {
            _dropsMotes = true;
        }
    }

    public void Kill(bool shadow)
    {
        if(_dropsMotes)
        {
            DropMotes(shadow);
        }
        Destroy(gameObject);
    }

    public void SetMoteDrops(bool drops)
    {
        _dropsMotes = drops;
    }

    void DropMotes(bool shadow)
    {
        // Drop motes
        Debug.Log("Mote drop " + shadow.ToString());
    }
}

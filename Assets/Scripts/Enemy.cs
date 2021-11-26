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
            // Drop motes
            Debug.Log("Mote drop " + shadow.ToString());
        }
        Destroy(gameObject);
    }

    public void SetMoteDrops(bool drops)
    {
        _dropsMotes = drops;
    }
}

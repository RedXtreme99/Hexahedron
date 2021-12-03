using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    // Components
    [SerializeField] MouseLook _persp;
    [SerializeField] FireWeapon _weapon;
    [SerializeField] GameObject _Mote;
    [SerializeField] Transform _fullMotePoint;

    // UI Components
    [SerializeField] Text _moteText;

    // Class variables
    int _motesHeld = 0;
    int _maxMotes = 5;
    float _moteTimer = 29.9f;
    bool _holdingMotes = false;
    bool _shadow = false;
    [HideInInspector]
    public bool _fullMotes = false;
    GameObject _fullIndicator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(_holdingMotes)
        {
            if(_moteTimer <= 0)
            {
                RemoveMotes();
            }
            else
            {
                _moteTimer -= Time.deltaTime;
                DisplayMoteText();
            }
            if(_fullMotes)
            {
                if(Input.GetKeyDown(KeyCode.Mouse1))
                {
                    PerformCleanse();
                }
            }
        }
        if(!_holdingMotes)
        {
            HideMoteText();
        }
    }

    public void AddMote(bool shadow)
    {
        if(!_fullMotes)
        {
            _holdingMotes = true;
            _moteTimer = 29.9f;
            if(shadow == _shadow)
            {
                _motesHeld++;
                if(_motesHeld == _maxMotes)
                {
                    SetFullMotes();
                }
            }
            else
            {
                _shadow = shadow;
                _motesHeld = 1;
            }
        }
    }

    void SetFullMotes()
    {
        _fullMotes = true;
        _fullIndicator = Instantiate(_Mote,
            _fullMotePoint.position, transform.rotation,
            gameObject.transform);
        _fullIndicator.GetComponent<Mote>().SetDark(_shadow);
        _weapon.SetDisabled(true);
        _persp.TogglePerspective();
    }

    void RemoveMotes()
    {
        _motesHeld = 0;
        _holdingMotes = false;
        if(_fullMotes)
        {
            _persp.TogglePerspective();
        }
        _fullMotes = false;
        _weapon.SetDisabled(false);
        Destroy(_fullIndicator);
    }

    void PerformCleanse()
    {
        string darkString = _shadow ? "dark" : "light";
        Debug.Log("CLEANSE " + darkString);
        RemoveMotes();
    }

    void DisplayMoteText()
    {
        string displayText = "";
        if(_shadow)
        {
            displayText += "Dark Motes ";
        }
        else
        {
            displayText += "Light Motes ";
        }
        if(_fullMotes)
        {
            displayText += "Max ";
        }
        else
        {
            displayText += "x" + _motesHeld.ToString() + " ";
        }
            displayText += "0:" + string.Format("{0:00}", _moteTimer);
        _moteText.text = displayText;
    }

    void HideMoteText()
    {
        _moteText.text = "";
    }
}

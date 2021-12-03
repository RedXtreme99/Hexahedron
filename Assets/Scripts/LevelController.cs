using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    // Instance of this script for singleton pattern
    public static LevelController Instance = null;

    // Components
    [SerializeField] GameObject _cubeRoom;
    [SerializeField] GameObject[] _faces;
    [SerializeField] GameObject _target;
    [SerializeField] Transform[] _targetPoints;
    [SerializeField] Transform[] _tpPoints;
    [SerializeField] FlashImage _flashImage;
    [SerializeField] GameObject _player;
    [SerializeField] Text _winText;

    // Accessible level traits
    [HideInInspector]
    public Light _levelLight;
    [HideInInspector]
    public bool _paused;
    [HideInInspector]
    public bool _transitioning;

    // Class variables
    public Floor _currentFloor = Floor.Bottom;
    Rotation _nextRotation = Rotation.None;
    int _progress = 0;
    bool _active;

    // Enums for game state and rotations
    public enum Floor { Bottom, Right, Front, Left, Back, Top };
    public enum Rotation { None, Clockwise, Forward, Counterclockwise, Backward };

    void Awake()
    {
        // Singleton pattern, ensure only one of this script exists and
        // persists between scene reloads
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _levelLight = FindObjectOfType<Light>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // On start, lock and hide mouse cursor
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _paused = false;
        _target.transform.position = _targetPoints[(int)_currentFloor].position;
        _target.transform.rotation = _targetPoints[(int)_currentFloor].rotation;
        ActivateFountains();
    }

    void Update()
    {
        // Ensure level light is connected
        if(_levelLight == null)
        {
            _levelLight = FindObjectOfType<Light>();
        }

        // Capture tab input to toggle pause
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(_paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }

        // Capture backspace input to restart game
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            RestartGame();
        }

        // Capture escape input to quit game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            QuitLevel();
        }
    }

    // Lock and hide cursor and set timestep to 1 for unpause
    void Unpause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        _paused = false;
    }

    // Unlock and show cursor and stop time to pause
    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        _paused = true;
    }

    // Restart game by reloading scene
    void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    // Quit game from editor or application
    void QuitLevel()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void ReachedTarget()
    {
        _progress++;
        Debug.Log("Current progress: " + _progress.ToString());
        if(_progress < 6)
        {
            int randInt = Random.Range(0, 5);
            if(randInt >= (int)_currentFloor)
            {
                randInt++;
            }
            _target.transform.position = _targetPoints[randInt].position;
            _target.transform.rotation = _targetPoints[randInt].rotation;
        }
        else
        {
            _flashImage.StartFlash(2f, .8f, Color.green);
            _winText.enabled = true;
        }
    }

    void ActivateFountains()
    {
        if(!_active)
        {
            Fountain[] fountains = _faces[(int)_currentFloor].GetComponentsInChildren<Fountain>();
            foreach(Fountain fountain in fountains)
            {
                fountain.SetActive(true);
            }
            EnemySpawns spawns = _faces[(int)_currentFloor].GetComponentInChildren<EnemySpawns>();
            spawns.SetActive(true);
            _active = true;
        }
    }

    void DeactivateFountains()
    {
        Fountain[] fountains = _faces[(int)_currentFloor].GetComponentsInChildren<Fountain>();
        foreach(Fountain fountain in fountains)
        {
            fountain.SetActive(false);
        }
        EnemySpawns spawns = _faces[(int)_currentFloor].GetComponentInChildren<EnemySpawns>();
        spawns.SetActive(false);
        _active = false;
    }

    public void SetRotation(Rotation rotation)
    {
        DeactivateFountains();
        _nextRotation = rotation;
        _faces[(int)_currentFloor].GetComponentInChildren<CenterPlate>().SetActive(true);
        EnemySpawns spawns = _faces[(int)_currentFloor].GetComponentInChildren<EnemySpawns>();
        spawns.SetActive(false);
    }

    public void StartRotation()
    {
        _transitioning = true;
        StartCoroutine(RotateRoom());
    }

    IEnumerator RotateRoom()
    {
        yield return new WaitForSeconds(1);
        _flashImage.StartFlash(1, 1, Color.white);
        _faces[(int)_currentFloor].GetComponentInChildren<CenterPlate>().SetActive(false);
        Quaternion startingRotation = _cubeRoom.transform.rotation;
        Vector3 targetRotation = Vector3.zero;
        float elapsedTime = 0f;
        if(_nextRotation == Rotation.Forward)
        {
            targetRotation = 90f * Vector3.right;
        }
        else if(_nextRotation == Rotation.Backward)
        {
            targetRotation = -90f * Vector3.right;
        }
        else if(_nextRotation == Rotation.Clockwise)
        {
            targetRotation = -90f * Vector3.forward;
        }
        else if(_nextRotation == Rotation.Counterclockwise)
        {
            targetRotation = 90f * Vector3.forward;
        }
        else
        {
            Debug.Log("Error determining rotation");
        }
        SetNewFloor();
        _nextRotation = Rotation.None;
        while(elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            _cubeRoom.transform.rotation = Quaternion.Slerp(startingRotation, Quaternion.Euler(targetRotation), elapsedTime);
            yield return new WaitForEndOfFrame();
        }
        _transitioning = false;
        _player.GetComponent<CharacterController>().enabled = false;
        _player.transform.position = _tpPoints[(int)_currentFloor].position;
        _player.GetComponent<CharacterController>().enabled = true;
        ActivateFountains();
        yield return 0;
    }

    void SetNewFloor()
    {
        if(_nextRotation != Rotation.None)
        {
            switch(_currentFloor)
            {
                case Floor.Bottom:
                    switch(_nextRotation)
                    {
                        case Rotation.Forward:
                            _currentFloor = Floor.Front;
                            return;
                        case Rotation.Counterclockwise:
                            _currentFloor = Floor.Left;
                            return;
                        case Rotation.Clockwise:
                            _currentFloor = Floor.Right;
                            return;
                        case Rotation.Backward:
                            _currentFloor = Floor.Back;
                            return;
                        default:
                            Debug.Log("Error rotating from bottom floor");
                            return;
                    }
                case Floor.Right:
                    switch(_nextRotation)
                    {
                        case Rotation.Forward:
                            _currentFloor = Floor.Front;
                            return;
                        case Rotation.Counterclockwise:
                            _currentFloor = Floor.Bottom;
                            return;
                        case Rotation.Clockwise:
                            _currentFloor = Floor.Top;
                            return;
                        case Rotation.Backward:
                            _currentFloor = Floor.Back;
                            return;
                        default:
                            Debug.Log("Error rotating from right floor");
                            return;
                    }
                case Floor.Front:
                    switch(_nextRotation)
                    {
                        case Rotation.Forward:
                            _currentFloor = Floor.Top;
                            return;
                        case Rotation.Counterclockwise:
                            _currentFloor = Floor.Left;
                            return;
                        case Rotation.Clockwise:
                            _currentFloor = Floor.Right;
                            return;
                        case Rotation.Backward:
                            _currentFloor = Floor.Bottom;
                            return;
                        default:
                            Debug.Log("Error rotating from front floor");
                            return;
                    }
                case Floor.Left:
                    switch(_nextRotation)
                    {
                        case Rotation.Forward:
                            _currentFloor = Floor.Front;
                            return;
                        case Rotation.Counterclockwise:
                            _currentFloor = Floor.Top;
                            return;
                        case Rotation.Clockwise:
                            _currentFloor = Floor.Bottom;
                            return;
                        case Rotation.Backward:
                            _currentFloor = Floor.Back;
                            return;
                        default:
                            Debug.Log("Error rotating from right floor");
                            return;
                    }
                case Floor.Back:
                    switch(_nextRotation)
                    {
                        case Rotation.Forward:
                            _currentFloor = Floor.Bottom;
                            return;
                        case Rotation.Counterclockwise:
                            _currentFloor = Floor.Left;
                            return;
                        case Rotation.Clockwise:
                            _currentFloor = Floor.Right;
                            return;
                        case Rotation.Backward:
                            _currentFloor = Floor.Top;
                            return;
                        default:
                            Debug.Log("Error rotating from right floor");
                            return;
                    }
                case Floor.Top:
                    switch(_nextRotation)
                    {
                        case Rotation.Forward:
                            _currentFloor = Floor.Back;
                            return;
                        case Rotation.Counterclockwise:
                            _currentFloor = Floor.Left;
                            return;
                        case Rotation.Clockwise:
                            _currentFloor = Floor.Right;
                            return;
                        case Rotation.Backward:
                            _currentFloor = Floor.Front;
                            return;
                        default:
                            Debug.Log("Error rotating from right floor");
                            return;
                    }
                default:
                    Debug.Log("Error setting floor");
                    return;
            }
        }
    }
}

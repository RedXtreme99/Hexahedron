using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    // Instance of this script for singleton pattern
    public static LevelController Instance = null;

    // Components
    [SerializeField] GameObject _cubeRoom;
    [SerializeField] GameObject[] _faces;

    // Accessible level traits
    [HideInInspector]
    public Light _levelLight;
    [HideInInspector]
    public bool _paused;

    // Class variables
    Floor _currentFloor = Floor.Bottom;
    Rotation _nextRotation = Rotation.None;
    int _progress = 0;

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

    void ActivateFountains()
    {

    }

    void DeactivateFountains()
    {

    }

    public void SetRotation(Rotation rotation)
    {
        DeactivateFountains();
        _nextRotation = rotation;
        _faces[(int)_currentFloor].GetComponentInChildren<CenterPlate>().SetActive(true);
    }

    public void RotateRoom()
    {
        Debug.Log(_nextRotation.ToString());
        _faces[(int)_currentFloor].GetComponentInChildren<CenterPlate>().SetActive(false);
    }

    public void ReachedTarget()
    {
        _progress++;
    }
}

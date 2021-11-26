using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    // Instance of this script for singleton pattern
    public static LevelController Instance = null;

    // Accessible level traits
    [HideInInspector]
    public Light _levelLight;
    public static bool _paused;

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip _startingSong = null;

    public static AudioManager Instance = null;

    AudioSource _audioSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(_startingSong != null)
        {
            AudioManager.Instance.PlaySong(_startingSong);
        }
    }

    private void Update()
    {
        if(!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void PlaySong(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}

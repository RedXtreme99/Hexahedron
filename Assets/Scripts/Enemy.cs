using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject _Mote;
    [SerializeField] AudioClip _spawnSound;
    [SerializeField] AudioClip _deathSound;
    [SerializeField] ParticleSystem _particles;

    bool _dropsMotes = false;

    void Awake()
    {
        AudioManager.Instance.PlaySound(_spawnSound);
        _particles.Play();
    }

    public void Kill(bool shadow)
    {
        if(_dropsMotes)
        {
            DropMotes(shadow);
        }
        AudioManager.Instance.PlaySound(_deathSound);
        ParticleSystem particles = Instantiate(_particles, transform);
        particles.Play();
        Destroy(particles, 2);
        Destroy(gameObject);
    }

    public void SetMoteDrops(bool drops)
    {
        _dropsMotes = drops;
    }

    void DropMotes(bool shadow)
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject mote = Instantiate(_Mote,
                transform.position + i / 2 * Vector3.up, Quaternion.identity);
            mote.GetComponent<Mote>().SetDark(shadow);
            Rigidbody rb = mote.AddComponent<Rigidbody>();
            float randomX = Random.Range(-1f, 1f);
            float randomZ = Random.Range(-1f, 1f);
            rb.AddForce(new Vector3(randomX, 8f, randomZ));
        }
    }

    public void KillNoMotes()
    {
        Destroy(gameObject);
    }
}

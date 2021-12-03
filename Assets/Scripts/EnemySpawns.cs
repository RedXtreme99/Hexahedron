using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawns : MonoBehaviour
{
    [SerializeField] GameObject _upperEnemy;
    [SerializeField] GameObject _lowerEnemy;
    [SerializeField] Transform[] _spawnPoints;
    bool _active = false;
    GameObject _enemy1;
    GameObject _enemy2;
    bool _upper1 = true;
    bool _upper2 = true;

    public void SetActive(bool active)
    {
        _active = active;
        if(active)
        {
            int rand1 = Random.Range(0, 2);
            int rand2 = Random.Range(2, 4);
            _enemy1 = Instantiate(_upperEnemy, _spawnPoints[rand1].position, Quaternion.identity);
            _enemy2 = Instantiate(_upperEnemy, _spawnPoints[rand2].position, Quaternion.identity);
        }
        else
        {
            Destroy(_enemy1);
            Destroy(_enemy2);
        }
    }

    void Update()
    {
        if(_enemy1 == null && _active)
        {
            _upper1 = !_upper1;
            int rand1 = Random.Range(0, 2);
            if(_upper1)
            {
                _enemy1 = Instantiate(_upperEnemy, _spawnPoints[rand1].position, Quaternion.identity);
            }
            else
            {
                _enemy1 = Instantiate(_lowerEnemy, _spawnPoints[rand1 + 4].position, Quaternion.identity);
                _enemy1.GetComponent<Enemy>().SetMoteDrops(true);
            }
        }
        if(_enemy2 == null && _active)
        {
            _upper2 = !_upper2;
            int rand2 = Random.Range(2, 4);
            if(_upper2)
            {
                _enemy2 = Instantiate(_upperEnemy, _spawnPoints[rand2].position, Quaternion.identity);
            }
            else
            {
                _enemy2 = Instantiate(_lowerEnemy, _spawnPoints[rand2 + 4].position, Quaternion.identity);
                _enemy2.GetComponent<Enemy>().SetMoteDrops(true);
            }
        }
    }
}

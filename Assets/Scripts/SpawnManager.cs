using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab, _enemyContainer, _powerUpContainer, Player;
    [SerializeField] private GameObject _trippleShot, _speedBurst, _sheildsUp;
    private bool _gameOver = false;


    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }


    IEnumerator EnemySpawnRoutine () {
        Vector3 randomStartPosition = new Vector3(Random.Range(-9, 9), 8, 0);
        
        while (!_gameOver && _enemyPrefab != null) 
        {
            GameObject newEnemy = Instantiate<GameObject>(_enemyPrefab, randomStartPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(1.0f, 4.0f));

        }

        if (!_gameOver) 
        {
            Debug.Log("No enemy prefab found");
        }
        
        StopCoroutine(EnemySpawnRoutine());
    }


    private IEnumerator PowerUpSpawnRoutine ()
    {
        Vector3 randomStartPosition = new Vector3(Random.Range(-9, 9), 8, 0);

        while (!_gameOver && _trippleShot != null && _speedBurst != null && _sheildsUp != null) 
        {
            yield return new WaitForSeconds(Random.Range(10f, 20f));
            GameObject _powerUp = Instantiate(_trippleShot, randomStartPosition, Quaternion.identity);
            _powerUp.transform.parent = _powerUpContainer.transform;

        }
        
        if (!_gameOver) 
        {
            Debug.Log("No enemy prefab found");
        }

        StopCoroutine(PowerUpSpawnRoutine());

    }


    public void GameOver() {
        _gameOver = true;
        Destroy(_enemyContainer.GetComponentInChildren<Transform>().gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab, _enemyContainer, Player;
    private bool _gameOver = false;


    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine () {
        Vector3 randomEnemyPosition = new Vector3(Random.Range(-9, 9), 8, 0);
        
        while (!_gameOver && _enemyPrefab != null) 
        {
            GameObject newEnemy = Instantiate<GameObject>(_enemyPrefab, randomEnemyPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(1.0f, 4.0f));


            if (_gameOver)
            {
                Destroy(_enemyContainer.GetComponentInChildren<Transform>());
            }

        }

        if (!_gameOver) 
        {
            Debug.Log("No enemy prefab found");
        }
        
        StopCoroutine(EnemySpawnRoutine());
    }

    public void GameOver() {
        _gameOver = true;
    }
}

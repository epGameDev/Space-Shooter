using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab, _enemyContainer;
    [SerializeField] private GameObject[] _powerUps;
    private bool _gameOver = false;


    IEnumerator EnemySpawnRoutine () {

        
        while (!_gameOver && _enemyPrefab != null) 
        {

            yield return new WaitForSeconds(Random.Range(1.0f, 4.0f));
            Vector3 randomStartPosition = new Vector3(Random.Range(-9, 9), 8, 0);
            GameObject newEnemy = Instantiate<GameObject>(_enemyPrefab, randomStartPosition, Quaternion.identity);
            if (_enemyContainer != null)
            {
                newEnemy.transform.parent = _enemyContainer.transform;
            }

        }
 
        StopCoroutine(EnemySpawnRoutine());

    }


    private IEnumerator PowerUpSpawnRoutine ()
    {
        Vector3 randomStartPosition = new Vector3(Random.Range(-9, 9), 8, 0);

        while (!_gameOver && _powerUps != null) 
        {
            yield return new WaitForSeconds(Random.Range(10f, 20f));
            Instantiate(_powerUps[Random.Range(0, _powerUps.Length)], randomStartPosition, Quaternion.identity);

        }
        

        StopCoroutine(PowerUpSpawnRoutine());

    }

    public void StopSpawning() {
        _gameOver = true;

        StopAllCoroutines();

        foreach (Transform child in _enemyContainer.transform)
        {
            Destroy(child.gameObject);
        }

    }

    public void StartGame()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }
}

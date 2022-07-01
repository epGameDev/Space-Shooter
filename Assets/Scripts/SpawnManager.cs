using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab, _enemyContainer;
    private bool gameOver = false;


    void Start()
    {
        StartCoroutine(EnemySpawnRoutine());
    }

    IEnumerator EnemySpawnRoutine () {
        Vector3 randomEnemyPosition = new Vector3(Random.Range(-9, 9), 8, 0);
        
        while (!gameOver && _enemyPrefab != null) 
        {
            GameObject newEnemy = Instantiate<GameObject>(_enemyPrefab, randomEnemyPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(2.0f, 6.0f));
        }
    }
}

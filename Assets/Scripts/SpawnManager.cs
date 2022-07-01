using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject _enemyPrefab;
    private bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        enemy = _enemyPrefab.GetComponent<Enemy>();
        StartCoroutine(EnemySpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemySpawnRoutine () {
        
        while (!gameOver && enemy != null) 
        {
            Instantiate<GameObject>(_enemyPrefab, new Vector3(enemy.randomSpawnLocation, 8, 0), enemy.transform.rotation);
            yield return new WaitForSeconds(5);
        }
    }
}

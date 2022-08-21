using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject _enemyContainer, _newEnemy, _bossPrefab;
    [SerializeField] private GameObject[] _powerUps, _enemyPrefab;
    private bool _canSpawn = true;
    

    // ====================================== //
    // ============== Spawners ============== //

    IEnumerator EnemySpawnRoutine (float minTime, float maxTime, bool isBossBattle = false) 
    {
        // TODO: Fix _canspawn so the powerups still spawn. 
        // TODO: Add other enemys to the normal attack of the boss.
        // TODO: Get more sleep. 😴️
        
        while (_canSpawn && _enemyPrefab != null) 
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime)); // 4, 6
            Vector3 randomStartPosition = new Vector3(Random.Range(-9, 9), 8, 0);
            if (_enemyContainer.transform.childCount >= 4 && isBossBattle) { _canSpawn = false; }
            

            if (isBossBattle && _enemyContainer.transform.childCount < 4)
            {
                _newEnemy = Instantiate<GameObject>(_enemyPrefab[_enemyPrefab.Length-1], randomStartPosition, Quaternion.identity);
            }
            else if (!isBossBattle)
            {
                _newEnemy = Instantiate<GameObject>(_enemyPrefab[Random.Range(0, _enemyPrefab.Length)], randomStartPosition, Quaternion.identity);
            }

            if (_enemyContainer != null)
            {
                _newEnemy.transform.parent = _enemyContainer.transform;

            }

            _newEnemy.GetComponent<Enemy>().EnablePowerUp();
        }
        
        StopCoroutine(EnemySpawnRoutine(minTime, maxTime));

    }

    private IEnumerator PowerUpSpawnRoutine (float minTime, float maxTime)
    {
        Vector3 randomStartPosition = new Vector3(Random.Range(-9, 9), 8, 0);

        while (_canSpawn && _powerUps != null) 
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime)); // 10, 20
            Instantiate(_powerUps[Random.Range(0, _powerUps.Length)], randomStartPosition, Quaternion.identity);
        }
        

        StopCoroutine(PowerUpSpawnRoutine(minTime, maxTime));

    }

    // =============================================== //
    // ============== Spawn Controllers ============== //

    public void StartGame(float minEnemy, float maxEnemy, float minPowerUp, float maxPowerUp, bool isBossBattle = false)
    {
        StopSpawning(); // Used to delete all enemies and stop coroutines before starting back up.
        _canSpawn = true;
        StartCoroutine(EnemySpawnRoutine(minEnemy, maxEnemy, isBossBattle));
        StartCoroutine(PowerUpSpawnRoutine(minPowerUp, maxPowerUp));
    }

    public void StopSpawning() 
    {
        _canSpawn = false;
        StopAllCoroutines();

        foreach (Transform child in _enemyContainer.transform)
        {
            Destroy(child.gameObject);
        }

    }

    public bool CheckChildCount(bool isBossBattle)
    {
        if (isBossBattle && _enemyContainer.transform.childCount <= 0 && _canSpawn == false)
        {
            // _bossPrefab.GetComponent<BossManager>().StopSwarm();
            return true;

        }
        return false;
    }
}
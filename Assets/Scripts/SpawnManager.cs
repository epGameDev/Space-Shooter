using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    
    
    //=====================================//
    //========= Private Variables =========//

    [SerializeField] private GameObject _enemyContainer, _newEnemy, _bossPrefab;
    [SerializeField] private GameObject[] _powerUps, _enemyPrefab;
    [SerializeField] private float _leftBounds, _rightBounds;
    private int _swarmCounter;
    private bool _canSpawnEnemy, _canSpawnPowerUp, _isBossBattle, _swarmState;

    
    
    
    //=================================//
    //========= Unity Methods =========//

    private void Start() 
    {
        _leftBounds = -11f;
        _rightBounds = 11f;

        _swarmCounter = 10;
        _canSpawnEnemy = true;
        _canSpawnPowerUp = true;
        _isBossBattle = false;
        _swarmState = false;

        StartGame(100, 100, 2, 2, false);
    }
    



    // ====================================== //
    // ============== Spawners ============== //

    IEnumerator EnemySpawnRoutine (float minTime, float maxTime, bool isBossBattle = false) 
    {        

        while (_canSpawnEnemy && _enemyPrefab != null) 
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime)); // 4, 6
            Vector3 randomStartPosition = new Vector3(Random.Range(_leftBounds, _rightBounds), 8, 0);
            if (_swarmCounter >= 6) { _swarmState = false; }
            
            switch (isBossBattle)
            {
                case true:
                    // Boss Battle State
                    if (_swarmState && _swarmCounter <= 6) // Only For Swarm State
                    {
                        _newEnemy = Instantiate<GameObject>(_enemyPrefab[_enemyPrefab.Length-1], randomStartPosition, Quaternion.identity);
                        _swarmCounter++;
                    }
                    else if (isBossBattle && _enemyContainer.transform.childCount <= 3) // Boss State If No Swarm State
                    {
                        _newEnemy = Instantiate<GameObject>(_enemyPrefab[Random.Range(0, _enemyPrefab.Length - 1)], randomStartPosition, Quaternion.identity);
                    }
                    break;
                
                case false:
                    // Normal State
                    Debug.Log("Normal State");
                    _newEnemy = Instantiate<GameObject>(_enemyPrefab[Random.Range(0, _enemyPrefab.Length-1)], randomStartPosition, Quaternion.identity);
                    break;
            }

            if (_enemyContainer != null)
            {
                _newEnemy.transform.parent = _enemyContainer.transform;
            }

            _newEnemy.GetComponent<Enemy>().EnablePowerUp();
        }

        _swarmCounter = 0;
        StopCoroutine(EnemySpawnRoutine(minTime, maxTime));

    }


    private IEnumerator PowerUpSpawnRoutine (float minTime, float maxTime)
    {
        Vector3 randomStartPosition = new Vector3(Random.Range(-9, 9), 8, 0);

        while (_canSpawnPowerUp && _powerUps != null) 
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime)); // 10, 20
            // Instantiate(_powerUps[Random.Range(0, _powerUps.Length)], randomStartPosition, Quaternion.identity);
            Instantiate(_powerUps[8], randomStartPosition, Quaternion.identity);
        }
        

        StopCoroutine(PowerUpSpawnRoutine(minTime, maxTime));

    }




    // =============================================== //
    // ============== Spawn Controllers ============== //

    public void StartGame(float minEnemy, float maxEnemy, float minPowerUp, float maxPowerUp, bool isBossBattle = false)
    {
        if (!_isBossBattle){
            StopSpawning(); // Used to delete all enemies and stop coroutines before starting back up.
        }
        else
        {
            StopAllCoroutines();
        }
        _canSpawnEnemy = true;
        _canSpawnPowerUp = true;
        StartCoroutine(EnemySpawnRoutine(minEnemy, maxEnemy, _isBossBattle));
        StartCoroutine(PowerUpSpawnRoutine(minPowerUp, maxPowerUp));
    }


    public void StopSpawning() 
    {
        _canSpawnEnemy = false;
        _canSpawnPowerUp = false;
        StopAllCoroutines();

        foreach (Transform child in _enemyContainer.transform)
        {
            Destroy(child.gameObject);
        }

    }

    
    
    
    //=======================================//
    //========= Boss Battle Methods =========//

    public bool CheckChildCount(bool isBossBattle)
    {
        if (isBossBattle && _swarmState == false)
        {
            // _bossPrefab.GetComponent<BossManager>().StopSwarm();
            return true;

        }
        return false;
    }


    public void IsBossBattle()
    {
        _isBossBattle = true;
        StopSpawning();
    }


    public void Swarm()
    {
        _swarmState = true;
        _swarmCounter = 0;
    }

}
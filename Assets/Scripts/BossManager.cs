using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    // ? Feature: [ ] The boss and fleet come in low and small as the score reaches 1500 and when the boss arives, size adjusts, enemies die, start.

public class BossManager : MonoBehaviour
{
    private GameManager _gameManager;
    [SerializeField] private Player _player;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private ParticleSystem _particleBossHealth;
    private Animator _animController;
    private AudioSource _explosionSound;
    private EdgeCollider2D _collider;

    
    
    
    //=====================================//
    //========= Private Variables =========//

    [SerializeField] private AnimationCurve _xMovementCurve, _aggressivLungeCurve;
    [SerializeField] private GameObject _primaryAttack, _secondaryAttack, _laserFirePos, _altFirePos, _particlePulseCanon, _darkProjectile;
    [SerializeField] GameObject[] _homingPos;
    private Vector3 _entryPos, _startPos, _centerAttackTarget;

    [SerializeField] private float _leftBounds, _rightBounds, _resetLocationY, _spawnLocationY, _xMovementDirection, _yMovementDirection, distance;
    [SerializeField] private float  _speed, _fireRate, _shotInterval, _altShotInterval, _distanceToEnemy;
    [SerializeField] float _health, _maxHealth, _regenerateRate;
    [SerializeField] private int _state, _nextState, _pulseCanonColiderSize;
    [SerializeField] private bool _gameOver, _madeEntry, _stateRoutineLoaded;


    
    
    
    //=================================//
    //========= Unity Methods =========//

    void Start()
    {
        _gameManager = GameManager.Instance;
        if (_gameManager == null) { Debug.Log("BossManager:: _gameManager is null"); }
    
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) { Debug.Log("BossManager:: _player is null"); }
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null) { Debug.Log("BossManager:: _spawnManager is null"); }
        
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null) { Debug.Log("BossManager:: _uiManager is null"); }
        

        _yMovementDirection = 1;
        _xMovementDirection = _xMovementDirection == 0 ? 1: -1;
        _centerAttackTarget = new Vector3(0,-2.72f, 0);
        _startPos = new Vector3(this.transform.position.x, 5.05f, 0);
        _entryPos = new Vector3(0, -12, 0);
        _leftBounds = -10.2f;
        _rightBounds = 10.2f;
        _pulseCanonColiderSize = 1;
        _gameOver = false;
        _madeEntry = false;
        _stateRoutineLoaded = false;
        _maxHealth = 5000f;
        _health = _maxHealth;

        _state = 0;
        _nextState = _state; 

        this.transform.position = _entryPos;
        this.transform.rotation = Quaternion.Euler(0, 0, 180);
    }


    void Update()
    {
        HealthCheck();

        if (_madeEntry)
        {
            AttackState(_state);
            _uiManager.UpdateBossHealth(_health, true);
        }
        else
        {
            EnterScene();
        }

        _uiManager.ShowUIDamage();
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {

        switch (other.gameObject.tag)
        {
            case "Player":
                _player.TakeDamage();
                _health -= 30;
                break;
            case "laser":
                _health -= 30;
                Destroy(other.gameObject);
                break;
            case "Bomb":
                _health -= 200;
                break;
            
            default:
            break;
        }

    }

    
    
    
    //===================================//
    //========= Starting Method =========//

    private void EnterScene()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(0, 12), (_speed - 3) * Time.deltaTime);

        if (this.transform.position.y == 12)
        {
            this.transform.rotation = Quaternion.FromToRotation(new Vector3(0, 0, 180), new Vector3(0, 0, 0));
            _madeEntry = true;
            this.GetComponent<EdgeCollider2D>().enabled = true;
            _spawnManager.IsBossBattle();
            _spawnManager.StartGame(5, 8, 4, 6, true);
        }
    }

    
    
    
    //====================================//
    //========= State Controller =========//

    private void AttackState(int attackState)
    {
        switch (attackState)
        {
            case 1: //========================================== Normal Attack State
                if (!_stateRoutineLoaded)
                {
                    StartCoroutine(StateTimer(5f, 10f));
                    StartCoroutine(WaveMovementRoutine());
                    StartCoroutine(FireRoutine());
                }
                NormalAttackState();
                break;

            case 2: //========================================== Agressive Attack State
                if(!_stateRoutineLoaded)
                {
                    StartCoroutine(StateTimer(3f, 3f));
                }
                AgressiveAttack();
                break;

            case 3: //========================================== Pulse Cannon State
                if (!_stateRoutineLoaded)
                {
                    StartCoroutine(StateTimer(14f, 14f));
                }
                PulseCanonState();
                // PulseSweep();
                break;

            case 4: //========================================== Swarm State
                if(!_stateRoutineLoaded)
                {
                    _spawnManager.StartGame(2f, 3f, 4f, 6f, true);
                    _spawnManager.Swarm();
                }
                SwarmState();
                break;

            case 5: //==========================================Homing Attack State
                if (!_stateRoutineLoaded)
                {
                    StartCoroutine(StateTimer(1f, 1f));
                    HomingAttackState();
                }
                break;
                
            case 6: //==========================================Fortify State
                if (!_stateRoutineLoaded)
                {
                    StartCoroutine(StateTimer(1f, 1f));
                    FortifyState();
                }

                break; 

            default: { //========================================== Default Load State
                if (_state != 0)
                {
                    Debug.LogError("Boss Battle:: Attack State Issue Detected " + _state);
                }
                BackToStart();
                break;
            }
        }
    }


    private void LoadNextState()
    {
        _state = Random.Range(1,7);

        // Reduces same state calls.
        if (_nextState == _state)
        {
            _state = Random.Range(1,7);
        }
        _nextState = _state;

    }


    private IEnumerator StateTimer(float min, float max)
    {
        yield return new WaitForSeconds(Random.Range(min, max));
        _state = 0;
    }


    private void BackToStart()
    {
        _startPos = new Vector3(this.transform.position.x, 5.05f, 0);
        _particlePulseCanon.SetActive(false);
        this.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (this.transform.position != _startPos)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, _startPos, _speed * Time.deltaTime);
        }
        else 
        {
            StopAllCoroutines();
            _stateRoutineLoaded = false;
            LoadNextState();            
        }
    }

    
    
    
    //=================================//
    //========= State Methods =========//

    private void BossMovement(float xDirection, float yDirection)
    {
        // Speed is more controlled through the animation curve in the inspector then the speed directly
        transform.Translate(new Vector3(xDirection, yDirection, 0).normalized * _xMovementCurve.Evaluate(_speed - 1) * Time.deltaTime);
        transform.position =  new Vector3(Mathf.Clamp(this.transform.position.x, _leftBounds, _rightBounds), this.transform.position.y, 0);
    }


    private void NormalAttackState()
    {
        if (this.transform.position.x == _rightBounds) 
        { 
            _xMovementDirection = -1f;
        }
        else if (this.transform.position.x == _leftBounds)
        {
            _xMovementDirection = 1f;
        }

        BossMovement(_xMovementDirection, _yMovementDirection);
    }


    private IEnumerator WaveMovementRoutine()
    {
        _stateRoutineLoaded = true;
        _yMovementDirection = 1f;
        while (!_gameOver)
        {
            yield return new WaitForSeconds(1f);
            _yMovementDirection = _yMovementDirection == 1 ? -1f : 1f;
        }
    }


    private IEnumerator FireRoutine()
    {
        _stateRoutineLoaded = true;

        while (!_gameOver){
            yield return new WaitForSeconds(_fireRate);
            Instantiate(_primaryAttack, _laserFirePos.transform.position, Quaternion.identity);
        }
    }

    private void AgressiveAttack()
    {
        _stateRoutineLoaded = true;
        _xMovementDirection = _xMovementDirection == 0 ? 1 : -1;
        _yMovementDirection = 0;
        this.transform.position = Vector2.Lerp(this.transform.position, _player.transform.position, _aggressivLungeCurve.Evaluate(_speed + 10) * Time.deltaTime);
    }

    private void SwarmState()
    {
        _stateRoutineLoaded = true;
        this.transform.position = Vector2.Lerp(this.transform.position, new Vector2(this.transform.position.x, 12), _aggressivLungeCurve.Evaluate(_speed) * Time.deltaTime);

        if (_spawnManager.CheckChildCount(true)) // TODO: [ ] Make sure only six enemies can spawn.
        {
            _state = 0;
        }
    }

    private void PulseCanonState()
    {
        _particlePulseCanon.SetActive(true);
        _particlePulseCanon.transform.GetChild(2).gameObject.SetActive(true);
        _particlePulseCanon.GetComponent<CapsuleCollider2D>().size.Set(3.8f, _pulseCanonColiderSize * Time.deltaTime);

        
        if (this.transform.position.x == _rightBounds) 
        { 
            _xMovementDirection = -1f;
        }
        else if (this.transform.position.x == _leftBounds)
        {
            _xMovementDirection = 1f;
        }

        BossMovement(_xMovementDirection, 0);
    }

    // ? Feature: [ ] Once shader graph is learned, this will be the new attack.
    private void PulseSweep()
    {
        _particlePulseCanon.SetActive(true);
        _particlePulseCanon.transform.GetChild(2).gameObject.SetActive(true);
        _particlePulseCanon.GetComponent<CapsuleCollider2D>().size.Set(3.8f, _pulseCanonColiderSize * Time.deltaTime);

        if (transform.position != new Vector3(0, 1, 0))
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(0, 1, 0), _speed * Time.deltaTime);
        }
        else 
        {
            this.transform.Rotate(new Vector3(0, 0, -1) * (_speed + 20) * Time.deltaTime, Space.Self);
        }
    }

    private void HomingAttackState()
    {
        // Projectiles have there own script to follow the player target.
        _stateRoutineLoaded = true;
        Instantiate(_darkProjectile, _homingPos[0].transform.position, Quaternion.identity);
        Instantiate(_darkProjectile, _homingPos[1].transform.position, Quaternion.identity);
    }

    private void FortifyState(int randomValue = 0)
    {
        _stateRoutineLoaded = true;
        _particleBossHealth.Play();
        randomValue = Random.Range(100, 401);
        _health += randomValue;
        _uiManager.BossHealthTextUpdate(randomValue);
    }


    private void HealthCheck()
    {
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }

        if (_health <= 0)
        {
            _gameOver = true;
            _gameManager.GameOver(_gameOver);
            _uiManager.DisplayGameOver("YOU WIN!!!");
            this.gameObject.SetActive(false);
        }
    }
 
}

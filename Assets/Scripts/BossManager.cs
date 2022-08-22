using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private Player _player;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private Animator _animController;
    private AudioSource _explosionSound;
    private EdgeCollider2D _collider;

    [SerializeField] private AnimationCurve _xMovementCurve, _aggressivLungeCurve;
    [SerializeField] private GameObject _primaryAttack, _secondaryAttack, _laserFirePos, _altFirePos, _pulseCanon, _playerTarget;
    private Vector3 _startPos, _centerAttackTarget;

    [SerializeField] private float _leftBounds, _rightBounds, _resetLocationY, _spawnLocationY, _xMovementDirection, _yMovementDirection, distance;
    [SerializeField] private float  _speed, _fireRate, _shotInterval, _altShotInterval, _distanceToEnemy;
    [SerializeField] float _health, _regenerateRate;
    [SerializeField] private int _state, _nextState;
    [SerializeField] private bool _gameOver, _stateRoutineLoaded;

    // TODO: Create a timer after all enemies have been instantiated and bring back in the boss
    // TODO: After boss is back in the game, lower the amount of powerups dropped. 
    // TODO: Create boss health bar.
    // TODO: Add 1 enemy at a time and one powerup per 10 seconds in regular mode.
    // TODO: Grow the enemy pulse canon collider over time so that the player isn't instantly damaged. 

    
    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        
        if (_player == null && _uiManager == null && _spawnManager == null)
        {
            Debug.LogError("Player/Manager Scripts not found");
        }

        _yMovementDirection = 1;
        _xMovementDirection = _xMovementDirection == 0 ? 1: -1;
        _centerAttackTarget = new Vector3(0,-2.72f, 0);
        _startPos = new Vector3(this.transform.position.x, 5.05f, 0);
        _gameOver = false;
        _stateRoutineLoaded = false;
        _health = 5000;

        _state = 0;
        _nextState = _state;
    }

    void Update()
    {
        AttackState(_state);
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
                if (_health <= 0)
                {
                    Destroy(this.gameObject);
                }
                break;
        }

    }

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
                    StartCoroutine(StateTimer(5f, 10f));
                    //// StartCoroutine(WaveMovementRoutine());
                }
                PulseCanonState();
                break;

            case 4: //========================================== Swarm State
                if(!_stateRoutineLoaded)
                {
                    _spawnManager.StopSpawning();
                    _spawnManager.StartGame(2f, 3f, 4f, 6f, true);
                }
                SwarmState();
                break;

            case 5: //==========================================Homing Attack State
                if (!_stateRoutineLoaded)
                {
                    StartCoroutine(StateTimer(5f, 10f));
                    StartCoroutine(WaveMovementRoutine());
                    StartCoroutine(FireRoutine());
                }
                NormalAttackState();
                break;
                
            case 6: //==========================================Fortify State
                if(!_stateRoutineLoaded)
                {
                    StartCoroutine(StateTimer(3f, 3f));
                }
                AgressiveAttack();
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
        _state = Random.Range(1,6);
        if (_nextState == _state)
        {
            _state = Random.Range(1,6);
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
        _pulseCanon.SetActive(false);

        if (this.transform.position != _startPos)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, _startPos, _xMovementCurve.Evaluate(_speed) * Time.deltaTime);
        }
        else 
        {
            StopAllCoroutines();
            _stateRoutineLoaded = false;
            LoadNextState();            
        }
    }
    
    private void BossMovement(float xDirection, float yDirection)
    {
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
        _xMovementDirection = _xMovementDirection == 0 ? 1 : -1;
        _yMovementDirection = 0;
        this.transform.position = Vector2.Lerp(this.transform.position, _playerTarget.transform.position, _aggressivLungeCurve.Evaluate(_speed + 10) * Time.deltaTime);
    }

    private void SwarmState()
    {
        _stateRoutineLoaded = true;
        this.transform.position = Vector2.Lerp(this.transform.position, new Vector2(this.transform.position.x, 12), _aggressivLungeCurve.Evaluate(_speed) * Time.deltaTime);

        if (_spawnManager.CheckChildCount(true))
        {
            _state = 0;
        }

    }

    private void PulseCanonState()
    {
        // if (!_pulseCanon.activeInHierarchy)
        // {
            _pulseCanon.SetActive(true);
        // }
        
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
    

}

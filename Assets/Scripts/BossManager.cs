using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private Player _player;
    private UIManager _uiManager;
    private Animator _animController;
    private AudioSource _explosionSound;
    private EdgeCollider2D _collider;
    [SerializeField] private AnimationCurve _xMovementCurve, _yMovementCurve;

    [SerializeField] private float _leftBounds, _rightBounds, _resetLocationY, _spawnLocationY, _xMovementDirection, _yMovementDirection;
    [SerializeField] private float  _speed, _fireRate, _shotInterval, _altShotInterval, _distanceToEnemy;
    [SerializeField] private GameObject _primaryAttack, _secondaryAttack, _laserFirePos, _pulseCanonPos;
    [SerializeField] float _health, _regenerateRate;
    private bool _gameOver, _attackStateComplete, _stateRoutineLoaded;


    
    void Start()
    {
        _yMovementDirection = 1;
        _xMovementDirection = 1;
        _gameOver = false;
        _attackStateComplete = true;
        _stateRoutineLoaded = false;
    }

    void Update()
    {
        AttackState(2);
    }

    private void AttackState(int attackState)
    {
        switch (attackState)
        {
            case 1:
                // Normal Attack State
                if (!_stateRoutineLoaded)
                {
                    StopAllCoroutines();
                    StartCoroutine(NormalAttackWaveRoutine());
                    StartCoroutine(FireRoutine());
                    Debug.Log("Loaded State Routine");
                }
                NormalAttackState();
                break;
            case 2:
                // Agressive Attack State
                Debug.Log("Agressive!!!!");
                break;
            case 3:
                // Pulse Cannon
                break;
            case 4:
                //Swarm
                break;
            case 5:
                //Homing Attack
                break;
            case 6: 
                //Fortify
                break;            
            default: {
                Debug.Log("Boss Battle:: Attack State Issue Detected");
                break;
            }
        }
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

        transform.Translate(new Vector3(_xMovementDirection, _yMovementDirection, 0).normalized * _xMovementCurve.Evaluate(_speed) * Time.deltaTime);
        transform.position =  new Vector3(Mathf.Clamp(this.transform.position.x, _leftBounds, _rightBounds), this.transform.position.y, 0);
    }

    private IEnumerator NormalAttackWaveRoutine()
    {
        _stateRoutineLoaded = true;
        while (!_gameOver)
        {
            yield return new WaitForSeconds(1f);
            _yMovementDirection = _yMovementDirection == 1 ? -1f : 1f;
            Debug.Log("Routine Updated");
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
}

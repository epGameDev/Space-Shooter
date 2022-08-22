using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _player ;
    private UIManager _uiManager;
    private Animator _anim;
    private AudioSource _explosionSound;
    private EdgeCollider2D _collider;

    [SerializeField] private AnimationCurve _curve;


    //=====================================//
    //========= Private Variables =========//
    [SerializeField] private float _leftBounds, _rightBounds, distance;
    [SerializeField] private float _speed = 4, _fireRate, _directionChange, _health = 100, _randomSpawnLocation;
    private float _timer, _altShotTimer, _distanceToEnemy = 0;
    [SerializeField] private int _playerPoints;
    [SerializeField] GameObject _laserPrefab, _firePos, _backFirePos, _altFire, _shield;
    private bool _hasDied, _shieldActive, _altShotEnabled = false;


    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = UIManager.Instance;
        _anim = this.GetComponent<Animator>();
        _explosionSound = this.gameObject.GetComponent<AudioSource>();
        _collider = gameObject.GetComponent<EdgeCollider2D>();
        _leftBounds = -11.58f;
        _rightBounds = 11.58f;
        _directionChange = 0;
        _fireRate = 0.9f;
        _hasDied = false;
        _timer = 0;
        _altShotTimer = 0;

        StartCoroutine(FireRoutine());
    }


    void Update()
    {
        EnemyMovement();
        if (_altShotEnabled) 
        {
            EnableBackShot();
        }
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {

        
        if (other.transform.tag == "laser") {

            Destroy(other.gameObject);
            ChangeDirection();

            EnemyFire();

            if(!_shieldActive)
             {
                _health -= 30;
             }
             else
             {
                _shield.SetActive(false);
                _shieldActive = false;
             }
        }


        if (_health <= 0 ) 
        {   
            SelfDestruct(true);
        }


        if (other.transform.name == "Player" )
        {
            if(_player != null) 
            {
                _player.TakeDamage();
            }

            SelfDestruct(false); 
        }
        
    }


    //==================================//
    //========= Custom Methods =========//

    void EnemyMovement() 
    {
        _randomSpawnLocation = Random.Range(_leftBounds, _rightBounds);

        if (_health <= 40)
        {
            ChargePlayer(6f);
        }
        else
        {
            transform.Translate( (new Vector3(_directionChange, -1f, 0f) * _speed) * Time.deltaTime);
        }


        transform.position = new Vector3 (Mathf.Clamp(this.transform.position.x, _leftBounds, _rightBounds), this.transform.position.y, 0);
        
        if(transform.position.y < -6f && !_hasDied) {
            transform.position = new Vector3(_randomSpawnLocation, 8, 0);
        }


    }

    public void EnemyFire()
    {
        if (_timer < Time.time)
        {
            _timer = Time.time + _fireRate;
            Instantiate(_laserPrefab, _firePos.transform.position, Quaternion.identity);
            
        }
    }

    private IEnumerator FireRoutine()
    {
        while (!_hasDied){
            yield return new WaitForSeconds(Random.Range(4f, 9f));
            Instantiate(_laserPrefab, _firePos.transform.position, Quaternion.identity);
        }
    }

    private void ChargePlayer(float attackRadius)
    {
        distance = Vector3.Distance(this.gameObject.transform.position, _player.gameObject.transform.position);

        if (distance < attackRadius && !_hasDied) 
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(_player.transform.position.x, this.transform.position.y - 10f, 0f), _curve.Evaluate(_speed + 10) * Time.deltaTime);
        }
        else
        {
            transform.Translate( (new Vector3(_directionChange, -1f, 0f) * _speed) * Time.deltaTime);
        }
        
    }


    public void ChangeDirection()
    {
        _directionChange = Random.Range(-1, 2);
        StartCoroutine(DirectionChangeDistance());
    }


    private IEnumerator DirectionChangeDistance()
    {   
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        _directionChange = 0;
    }


    public void EnablePowerUp()
    {
        float randomNumber = Mathf.Round(Random.Range(0, 10));

        if (randomNumber == 4)
        {
            EnableShields();
        }
        else if (randomNumber == 6)
        {
            _altShotEnabled = true;
        }
    }

    private void EnableShields()
    {
        _shield.SetActive (true);
        _shieldActive = true;
    }

    private void EnableBackShot()
    {

        if (this.transform.position.y < _player.transform.position.y && !_hasDied) 
        {
            _distanceToEnemy = Mathf.Abs(this.transform.position.x - _player.transform.position.x);

            if (_altShotTimer <= Time.time && _distanceToEnemy < 0.5)
            {
                _altShotTimer = Time.time + _fireRate + 6f;
                GameObject backShot = Instantiate(_altFire, _backFirePos.transform.position, Quaternion.identity);
                backShot.GetComponent<PowerUps>().ChangeSpeed(-3);
            }

        }
    }


    public void SelfDestruct(bool canGetPoints) 
    {
        if (canGetPoints)
        {
            _uiManager.DisplayPlayerScore(_playerPoints);
        }

        _hasDied = true;
        _altShotEnabled = false;
        _collider.enabled = false;
        _shield.SetActive(false);
        _speed = 3;
        StopAllCoroutines();
        _anim.SetTrigger("OnEnemyDeath");
        if (_explosionSound != null)
        {
            _explosionSound.Play();
        }
        Destroy(this.gameObject, 100f * Time.deltaTime);
    }

}
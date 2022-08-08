using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private UIManager _uiManager;
    private Animator _anim;
    private AudioSource _explosionSound;
    private EdgeCollider2D _collider;
    private CircleCollider2D _radar;
    [SerializeField] Transform _target;

    //=====================================//
    //========= Private Variables =========//
    [SerializeField] private float _leftBounds, _rightBounds;
    [SerializeField] private float _speed = 4, _fireRate, _directionChange, _health = 100, _randomSpawnLocation;
    private float _timer;
    [SerializeField] private int _playerPoints;
    [SerializeField] GameObject _laserPrefab, _firePos, _shield;
    private bool _hasDied, _shieldActive;


    void Start()
    {
        _uiManager = UIManager.Instance;
        _anim = this.GetComponent<Animator>();
        _explosionSound = this.gameObject.GetComponent<AudioSource>();
        _collider = gameObject.GetComponent<EdgeCollider2D>();
        _radar = gameObject.GetComponent<CircleCollider2D>();
        _target = GameObject.Find("Player").transform;
        _leftBounds = -9.1f;
        _rightBounds = 9.1f;
        _directionChange = 0;
        _fireRate = 0.9f;
        _hasDied = false;
        _timer = 0;

        StartCoroutine(FireRoutine());
    }


    void Update()
    {
        EnemyMovement();
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {

        
        if (other.transform.tag == "laser") {

            Destroy(other.gameObject);
            ChangeDirection();

            if (_timer < Time.time)
            {
                _timer = Time.time + _fireRate;
                Instantiate(_laserPrefab, _firePos.transform.position, Quaternion.identity);
            }

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
            Player player = other.GetComponent<Player>();

            if(player != null) 
            {
                player.Damage();
            }

            SelfDestruct(false);
            
        }
        
    }


    //==================================//
    //========= Custom Methods =========//

    void EnemyMovement() 
    {
        _randomSpawnLocation = Random.Range(_leftBounds, _rightBounds);

        transform.Translate( (new Vector3(_directionChange, -1f, 0f) * _speed) * Time.deltaTime);
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


    public void EnableShield()
    {
        float randomNumber = Mathf.Round(Random.Range(0, 10));
        if (randomNumber == 4)
        {
            _shield.SetActive (true);
            _shieldActive = true;
        }
    }


    public void SelfDestruct(bool canGetPoints) 
    {
        if (canGetPoints)
        {
            _uiManager.GetPlayerScore(_playerPoints);
        }

        _hasDied = true;
        _collider.enabled = false;
        _shield.SetActive(false);
        StopAllCoroutines();
        _anim.SetTrigger("OnEnemyDeath");
        if (_explosionSound != null)
        {
            _explosionSound.Play();
        }
        Destroy(this.gameObject, 100f * Time.deltaTime);
    }

}
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private UIManager _uiManager;
    private Animator _anim;
    private AudioSource _explosionSound;
    private EdgeCollider2D _collider;

    //=====================================//
    //========= Private Variables =========//
    [SerializeField] private float _leftBounds, _rightBounds;
    [SerializeField] private float _speed = 4, _health = 100, _randomSpawnLocation;
    [SerializeField] private int _playerPoints;
    [SerializeField] GameObject _laserPrefab, _firePos;
    private bool _hasDied;


    void Start()
    {
        _uiManager = UIManager.Instance;
        _anim = this.GetComponent<Animator>();
        _explosionSound = this.gameObject.GetComponent<AudioSource>();
        _collider = gameObject.GetComponent<EdgeCollider2D>();
        _leftBounds = -9.1f;
        _rightBounds = 9.1f;
        _hasDied = false;

        StartCoroutine(Fire());
    }


    void Update()
    {
        EnemyMovement();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.transform.tag == "laser") {
            Destroy(other.gameObject);
            _health -= 30;
        }

        if (_health <= 0 ) {
            _uiManager.GetPlayerScore(_playerPoints);
            SelfDestruct();
        }

        if (other.transform.name == "Player" )
        {
            Player player = other.GetComponent<Player>();

            if(player != null) 
            {
                player.Damage();
            }

            SelfDestruct();
            
        }
        
    }


    //==================================//
    //========= Custom Methods =========//

    void EnemyMovement() 
    {
        _randomSpawnLocation = Random.Range(_leftBounds, _rightBounds);

        transform.Translate( (Vector3.down * _speed) * Time.deltaTime);
        
        if(transform.position.y < -6f && !_hasDied) {
            transform.position = new Vector3(_randomSpawnLocation, 8, 0);
        }
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(Random.Range(6f, 9f));
        Instantiate(_laserPrefab, _firePos.transform.position, Quaternion.identity);
    }

    private void SelfDestruct()
    {
        _hasDied = true;
        _collider.enabled = false;
        StopAllCoroutines();
        _anim.SetTrigger("OnEnemyDeath");
        if (_explosionSound != null)
        {
            _explosionSound.Play();
        }
        Destroy(this.gameObject, 150f * Time.deltaTime);
    }

}

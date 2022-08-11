using UnityEngine;

public class Asteroid : MonoBehaviour
{

    GameManager _gameManager;
    CircleCollider2D _collider;

    [SerializeField] GameObject _explosion;
    [SerializeField] private float _rotateSpeed, _speed, _health;
    [SerializeField] private float _leftBounds, _rightBounds, _randomSpawnLocation;
    private bool _hasExploded;

    void Start()
    {
        _gameManager = GameManager.Instance;
        _collider = this.GetComponent<CircleCollider2D>();
        _health = 100;
        _rotateSpeed = 13f;
        _speed = 0.5f;
        _leftBounds = -9.1f;
        _rightBounds = 9.1f;
        _hasExploded = false;
    }

    void Update()
    {
        AsteroidMovement();
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "laser")
        {
            _health -= 30;
            Explode(this.gameObject);
            Destroy(other.gameObject);
            if(_health <= 0)
            {
                _collider.enabled = false;
                _gameManager.SetWave(0); //Wave determined by the player's points.
                _hasExploded = true;
                Destroy(this.gameObject, 10f * Time.deltaTime);
            }
        }

        if(other.gameObject.tag == "Player")
        {
            Explode(other.gameObject);
            Destroy(other.gameObject);
            if(_gameManager != null)
            {
                _gameManager.GameOver();
            }
        }
    }

        void AsteroidMovement() 
    {
        _randomSpawnLocation = Random.Range(_leftBounds, _rightBounds);

        transform.Rotate( new Vector3(0, 0, (1 * _rotateSpeed) * Time.deltaTime));
        transform.Translate((Vector3.down * _speed) * Time.deltaTime, Space.World);
        
        if(transform.position.y < -6f && !_hasExploded) {
            _health = 100;
            transform.position = new Vector3(_randomSpawnLocation, 8, 0);
        }
    }

    private void Explode(GameObject gameObject)
    {
        GameObject _explostionOBJ = Instantiate(_explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(_explostionOBJ.gameObject, 120f * Time.deltaTime);
    }
}

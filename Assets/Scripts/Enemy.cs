using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private UIManager _uiManager;

    //=====================================//
    //========= Private Variables =========//
    [SerializeField] private float _leftBounds, _rightBounds;
    [SerializeField] private float speed = 4, health = 100;
    [SerializeField] private int _playerPoints;

    public float randomSpawnLocation;

    private void Awake() {
        _uiManager = UIManager.Instance;
    }


    void Start()
    {
        _leftBounds = -9.1f;
        _rightBounds = 9.1f;
    }


    void Update()
    {
        EnemyMovement();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.transform.tag == "laser") {
            Destroy(other.gameObject);
            health -= 30;
        }

        if (health <= 0 ) {
            _uiManager.GetPlayerScore(_playerPoints);
            Destroy(this.gameObject);
        }

        if (other.transform.name == "Player" )
        {
            Player player = other.GetComponent<Player>();

            if(player != null) 
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }
        
    }


    //==================================//
    //========= Custom Methods =========//

    void EnemyMovement() 
    {
        randomSpawnLocation = Random.Range(_leftBounds, _rightBounds);

        transform.Translate( (Vector3.down * speed) * Time.deltaTime);
        
        if(transform.position.y < -6f) {
            transform.position = new Vector3(randomSpawnLocation, 8, 0);
        }
    }

}

using UnityEngine;

public class Enemy : MonoBehaviour
{
    //=====================================//
    //========= Private Variables =========//
    [SerializeField] private float _randomSpawnLocation, _leftBounds, _rightBounds;
    [SerializeField] private float speed = 4, health = 100;


    void Start()
    {
        _leftBounds = -9.1f;
        _rightBounds = 9.1f;
        _randomSpawnLocation = 0f;
    }


    void Update()
    {
        EnemyMovement();
    }


    private void OnTriggerEnter(Collider other) {
        if (other.transform.tag == "laser") {
            Destroy(other.gameObject);
            health -= 30;
        }

        if (health <= 0 ) {
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
        transform.Translate( (Vector3.down * speed) * Time.deltaTime);

        _randomSpawnLocation = Random.Range(_leftBounds, _rightBounds);
        
        if(transform.position.y < -6f) {
            transform.position = new Vector3(_randomSpawnLocation, 8, 0);
        }
    }
}

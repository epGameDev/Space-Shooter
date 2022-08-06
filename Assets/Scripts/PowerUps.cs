using UnityEngine;

public class PowerUps : MonoBehaviour
{
    // private AudioSource _audio;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private Transform _target;
    [SerializeField] private AnimationCurve _curve;

    [SerializeField] AudioClip _powerUpSound;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerUpID;
    private bool _moveTowardsPlayer;


    private void Start() 
    {
        _audioManager = AudioManager.Instance;
        _target = GameObject.Find("Player").transform;
        _moveTowardsPlayer = false;
    }


    void Update()
    {
        Movement();

        if (transform.position.y < -7f) {
            SelfDestruct();
        }
    }

    private void OnTriggerEnter2D (Collider2D other) 
    {
        if(other.transform.tag == "Player") 
        {

            Player player = other.transform.GetComponent<Player>();
            if (player != null) 
            {

                if(_audioManager != null)
                {
                    _audioManager.PlayPowerUpSFX(_powerUpSound);
                }

                switch (_powerUpID)
                {
                    case 0:
                        player.EnableTrippleShot();
                        break;
                    case 1:
                        player.EnableSpeedBoost();
                        break;
                    case 2:
                        player.PowerShields();
                        break;
                    case 3:
                        player.Heal();
                        break;
                    case 4:
                        player.LoadBombs();
                        break;
                    case 5:
                        player.DisableShip();
                        break;
                    default:
                        Debug.Log("No Power Ups Were Found");
                    break;
                }
            } 
            else 
            {
                Debug.Log("No Player Script Found");
            }
            
            SelfDestruct();
        }
    }

    private void Movement ()
    {
        if(Input.GetKeyDown(KeyCode.C) && _powerUpID != 5)
        {
            _moveTowardsPlayer = true;
        }
        
        if (_moveTowardsPlayer && _target != null)
        {
            this.transform.position = Vector2.Lerp(this.transform.position, _target.transform.position, _curve.Evaluate(_speed + 10) * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.down * _speed * Time.deltaTime);
        }

    }

    private void SelfDestruct () 
    {
        Destroy(this.gameObject);
    }
}
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private Transform _target;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] AudioClip _powerUpSound;

    
    
    
    //=====================================//
    //========= Private Variables =========//

    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerUpID;
    private bool _moveTowardsPlayer;

    
    
    
    //=================================//
    //========= Unity Methods =========//

    private void Start() 
    {
        _audioManager = AudioManager.Instance;
        if (_audioManager == null) { Debug.Log("PowerUps:: Audio Manager is null"); }
        
        _target = GameObject.Find("Player").transform;
        if (_target == null) { Debug.Log("PowerUps:: Player [_target] is null"); }
        
        _moveTowardsPlayer = false;
    }


    void Update()
    {
        Movement();

        if (this.transform.position.y < -7f || this.transform.position.y > 12) {
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
                Debug.Log("PowerUps:: Other.gameObject [player] is null");
            }
            
            SelfDestruct();
        }

        if (other.transform.tag == "EnemyFire" && _powerUpID != 5) // Destroys all but the EMP power up
        {
            Destroy(other.gameObject);
            SelfDestruct();
        }
        else if (other.gameObject.tag == "laser" && _powerUpID == 5) // Allows to destroy the EMP power up
        {
            Destroy(other.gameObject);
            SelfDestruct();
        }

        if (other.gameObject.tag == "PulseCanon") 
        {
            SelfDestruct();
        }


    }
    
    
    

    //=====================================//
    //========= Power Up Movement =========//

    private void Movement ()
    {
        if(Input.GetKeyDown(KeyCode.C) && _powerUpID != 5 && _target.gameObject.activeInHierarchy == true)
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

    public void ChangeSpeed(float speed)
    {
        _speed = speed;
    }

    
    
    
    //===============================//
    //========= End Of Life =========//

    private void SelfDestruct () 
    {
        Destroy(this.gameObject);
    }
}
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private CameraController _camera;
    private AudioSource _audio;




    //=====================================//
    //========= Private Variables =========//

    [SerializeField] private GameObject _laserPrefab, _tripleShotPrefab, _bombPrefab;
    [SerializeField] private GameObject _firePos1, _firePos2, _bombPos;
    [SerializeField] private GameObject _shield, _thruster, _particleSYS, _explosion;
    [SerializeField] private GameObject[] _engineDamage;
    [SerializeField] private float _leftBounds, _rightBounds, _upperBounds, _lowerBounds;
    [SerializeField] private float _timeSinceFired, _fireRate, _coolDown, _shotCount, _shotLimit;
    [SerializeField] private float _speed, _speedBurstLimit;
    [SerializeField] private int _shieldHealth, _bombCount;
    private int _maxBombs = 8;
    private Vector3 _startPos;
    [SerializeField] private bool canFire = true, _shieldEnabled = false, _tripleShotEnabled = false, _isDisabled = false;




    //====================================//
    //========= Public Variables =========//

    public int lives { get; private set; }




    //==============================//
    //========= Game Logic =========//

    void Start()
    {
        _gameManager = GameManager.Instance;
        _uiManager.GetComponent<UIManager>();
        if(_gameManager == null || _uiManager == null)
        {
            Debug.LogError("Player::UI/GameManager is null");
        }
        
        _camera = _camera.GetComponent<CameraController>();
        _audio = this.gameObject.GetComponent<AudioSource>();
        if (_audio == null || _camera == null)
        {
            Debug.LogError("Player::Audio/Camera refference is null");
        }

        _rightBounds = 11.58f;
        _leftBounds = -11.58f;
        _upperBounds = 6.85f;
        _lowerBounds = -5f;
        _startPos = new Vector3(0,-1.5f,0);
        _coolDown = 1f;
        _shotLimit = 30f;
        _shotCount = 0f;
        _bombCount = 0;
        _fireRate = 0.2f;
        lives = 3;
        _speed = 8f;

        // Set the start position and fire rate
        transform.position = _startPos;
        _timeSinceFired = Time.time + _fireRate;
       
       // Disable engine damage animation/prefab
       _engineDamage[0].SetActive(false);
       _engineDamage[1].SetActive(false);

        // (ammo ID, ammo count, max ammo)
        _uiManager.UpdateAmmoCount(0 , _bombCount, 8); // TODO: [ ] Add more ammo types

    }

 
    void Update()
    {
        // Can provide functions if the ship is not diabled.
        if (!_isDisabled)
        {
            PlayerMovement();
            FireLaser();
            LaunchBomb();
            SpeedBurst(12f);
        }

    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "EnemyFire")
        {
            TakeDamage();
            Destroy(other.gameObject);

            if (other.gameObject.name == "DarkProjectile")
            {
                ExplodeEffect();
                Destroy(other.gameObject);
                _camera.ShakeCamera();
            }
        }

        if (other.gameObject.tag == "PulseCanon")
        {
            TakeDamage();
            ExplodeEffect();
            _camera.ShakeCamera();
        }
    }




    // ====================================== //
    // ============== Movement ============== //

    private void PlayerMovement () 
    {
        // User Controls => GetAxis for smooth movement => GetAxisRaw for arcade movment.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Player Movement
        transform.Translate( (new Vector3(horizontalInput, verticalInput, 0).normalized * _speed ) * Time.deltaTime);

        // Player Restrictions
        Vector3 playerPos = transform.position;
        transform.position = new Vector3 (Mathf.Clamp(playerPos.x, _leftBounds, _rightBounds), Mathf.Clamp(playerPos.y, _lowerBounds, _upperBounds), 0);

    }


    private void SpeedBurst (float burstSpeed)
    {
        if (Input.GetButton("Fire3") && _speedBurstLimit > 0 ) 
        {
            // Displays Speed UI Meter
            _uiManager.UpdatePlayerBoostSpeed(_speedBurstLimit);

            _speed = burstSpeed;
            _speedBurstLimit -= 1 * Time.deltaTime;

            if (_speedBurstLimit <= 0) 
            {
                _speedBurstLimit = 0;
            }

        } 
        else 
        {
            _speed = 8f;
        }
    

    }




    // ==================================== //
    // ============== Attack ============== //

    private void FireLaser ()
    {  

        // Fire Conditions 
        if (_shotCount >= _shotLimit)
        { 
            canFire = false;
            _coolDown = 2f;
        }

        _uiManager.UpdatePlayerShotLimit(!canFire, _shotCount);


        // Fire Logic
        if (Input.GetButton("Fire1") && canFire && Time.time > _timeSinceFired)
        {
            if(_laserPrefab != null || _tripleShotPrefab != null)
            {
                _shotCount++;
                Instantiate(_laserPrefab, _firePos1.transform.position, Quaternion.identity);

                // Shoots two extra lasers
                if (_tripleShotEnabled) {
                    Instantiate(_tripleShotPrefab, _firePos2.transform.position, Quaternion.identity);
                }

                if(_audio != null)
                {
                    _audio.Play();
                }
            }

            _timeSinceFired = Time.time + _fireRate;

        }
        
        // Cool Down
        if (_shotCount > 0) 
        {
            _shotCount -= _coolDown * (Time.deltaTime / 0.60f);
        }
        else
        {
            _shotCount = 0;
            canFire = true;
            _coolDown = 1;
        }
    }


    private void LaunchBomb()
    {
        if (Input.GetButtonDown("Fire2") && _bombCount > 0 && _bombPrefab != null)
        {
            _bombCount--;
            Instantiate(_bombPrefab, _bombPos.transform.position, Quaternion.identity);
            _uiManager.UpdateAmmoCount(0 , _bombCount, 8);
        }
    }




    // ==================================== //
    // ============== Health ============== //

    public void TakeDamage()
    {
        

        if (_shieldEnabled)
        {
            _shieldHealth--;

            switch (_shieldHealth)
            {
                case 0:
                    _shield.SetActive(false);
                    _shieldEnabled = false;

                    break;
                case 1:     
                    _shield.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.25f);
                    break;
                case 2:
                    _shield.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                    break;
                default:
                    Debug.Log("Shield Health" + _shieldHealth);
                    break;
            }
        }
        else
        {
            lives--;
            if(lives > 0)
            {
                _camera.ShakeCamera();
            }
           
            HealthCheck();
        }

    }


    private void HealthCheck()
    {
        switch (lives)
        {
            case 0:
                _gameManager.GameOver();
                ExplodeEffect();
                this.gameObject.SetActive(false);
                _thruster.SetActive(false);
                break;
            case 1:
                _engineDamage[0].SetActive(true);
                _engineDamage[1].SetActive(true);
                break;
            case 2:
                _engineDamage[0].SetActive(true);
                _engineDamage[1].SetActive(false);
                break;
            case 3:
                _engineDamage[0].SetActive(false);
                _engineDamage[1].SetActive(false);
                break;
            case 4:
                lives = 3;
                break;
            default:
                lives = 0;
                Debug.LogError("Index for lives is out of range");
                break;
        }

        _uiManager.UpdatePlayerHealth(lives);
    }


    private void ExplodeEffect()
    {
        if(_explosion != null)
        {
            Instantiate(_explosion, this.transform.position, Quaternion.identity);
        }
    }




    // ======================================= //
    // ============== Power Ups ============== //

    public void EnableTrippleShot ()
    {
        _tripleShotEnabled = true;
        StartCoroutine(DisableTrippleShot());
    }

    private IEnumerator DisableTrippleShot ()
    {
        yield return new WaitForSeconds(5f);
        _tripleShotEnabled = false;
        StopCoroutine(DisableTrippleShot());
    }

    public void LoadBombs()
    {
        if (_bombCount < _maxBombs)
        {
            _bombCount += 2;
        }

        _uiManager.UpdateAmmoCount(0 , _bombCount, 8);
    }

    public void EnableSpeedBoost () 
    {
        _speedBurstLimit = 10;
        _uiManager.UpdatePlayerBoostSpeed(_speedBurstLimit);
    }

    public void DisableShip()
    {
        _particleSYS.SetActive(true);
        this.gameObject.GetComponentInChildren<ParticleSystem>().Play();
        _camera.ShakeCamera();
        _thruster.SetActive(false);

        if(!_isDisabled)
        {
            _isDisabled = true;
            StartCoroutine(DisableShipTimer());
        }
    }

    private IEnumerator DisableShipTimer()
    {
        yield return new WaitForSeconds(5f);
        _thruster.SetActive(true);
        _isDisabled = false;
    }

    public void PowerShields()
    {
        _shieldHealth = 3;
        _shieldEnabled = true;
        _shield.SetActive(true);
        _shield.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void Heal()
    {
        lives++;
        HealthCheck();
    }
}

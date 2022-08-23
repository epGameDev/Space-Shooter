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

    [SerializeField] private GameObject _laserPrefab, _tripleShotPrefab, _bombPrefab, _firePos1, _firePos2, _bombPos,  _shield, _thruster, _particleSYS;
    [SerializeField] private GameObject[] _engineDamage;
    [SerializeField] private float _leftBounds, _rightBounds, _upperBounds, _lowerBounds;
    private float _timeSinceFired, _fireRate, _coolDown, _shotCount, _shotLimit;
    [SerializeField] private float _speed, _speedBurstDuration;
    [SerializeField] private int _shieldHealth, _bombCount;
    private int _maxBombs = 8;
    private Vector3 _startPos;
    private bool canFire = true, _shieldEnabled = false, _tripleShotEnabled = false, _isDisabled = false;

    //====================================//
    //========= Public Variables =========//

    public int lives { get; private set; }


    void Start()
    {
        // Initialize Values
        _gameManager = GameManager.Instance;
        _uiManager.GetComponent<UIManager>();
        if(_gameManager == null || _uiManager == null)
        {
            Debug.LogError("Player::UI/GameManager is null");
        }

        _audio = this.gameObject.GetComponent<AudioSource>();

        _rightBounds = 11.58f;
        _leftBounds = -11.58f;
        _upperBounds = 6.85f;
        _lowerBounds = -5f;
        _startPos = new Vector3(0,-1.5f,0);
        _coolDown = 10f;
        _shotLimit = 30f;
        _shotCount = 0f;
        _bombCount = 0;
        _fireRate = 0.2f;
        lives = 3;
        _speed = 8f;

        // Set the start position and fire rate
        transform.position = _startPos;
        _timeSinceFired = Time.time + _fireRate;
       
       _engineDamage[0].SetActive(false);
       _engineDamage[1].SetActive(false);

       _shield = transform.GetChild(3).gameObject;
       _camera = _camera.GetComponent<CameraController>();
        _uiManager.UpdateAmmoCount(0 , _bombCount, 8);

    }

 
    void Update()
    {
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
        }

        if (other.gameObject.tag == "PulseCanon")
        {
            TakeDamage();
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
        if (Input.GetButton("Fire3") && _speedBurstDuration > 0 ) 
        {
            // Displays Speed UI Meter
            _uiManager.UpdatePlayerBoostSpeed(_speedBurstDuration);

            _speed = burstSpeed;
            _speedBurstDuration -= 1 * Time.deltaTime;

            if (_speedBurstDuration <= 0) 
            {
                _speedBurstDuration = 0;
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
        _uiManager.UpdatePlayerShotLimit(!canFire, _shotCount);

        // Fire Conditions 
        if (_shotCount < _shotLimit){ 
            canFire = true;
        }else{
            canFire = false;
            StartCoroutine(CoolDownTimer());
        }

        // Fire Logic
        if (Input.GetButton("Fire1") && canFire && Time.time > _timeSinceFired)
        {
            if(_laserPrefab != null)
            {
                Instantiate(_laserPrefab, _firePos1.transform.position, Quaternion.identity);
                if(_audio != null)
                {
                    _audio.Play();
                }
            }

            if (_tripleShotEnabled) {
                Instantiate(_tripleShotPrefab, _firePos2.transform.position, Quaternion.identity);
            }

            _timeSinceFired = Time.time + _fireRate;
            _shotCount++;
        }
        else if (_shotCount >=0 && canFire) {
            _shotCount -= 1 * (Time.deltaTime / 0.60f);
            if (_shotCount < 0) _shotCount = 0;
        }
    }

    private void LaunchBomb()
    {
        if (Input.GetButtonDown("Fire2") && _bombCount > 0)
        {
            _bombCount--;
            Instantiate(_bombPrefab, _bombPos.transform.position, Quaternion.identity);
            _uiManager.UpdateAmmoCount(0 , _bombCount, 8);

        }
    }


    IEnumerator CoolDownTimer () {

        yield return new WaitForSecondsRealtime(_coolDown);
        _shotCount = 0;
        StopCoroutine(CoolDownTimer());

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

        }else
        {
            lives--;
            if(lives > 0)
            {
                _camera.ShakeCamera();
            }
            
            HealthCheck();
        }

        
        if (lives <= 0) 
        {
            _gameManager.GameOver();
            this.gameObject.SetActive(false);
        }

    }

    private void HealthCheck()
    {
        switch (lives)
        {
            case 0:
                _gameManager.GameOver();
                this.gameObject.SetActive(false);
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
                Debug.LogError("Index for lives is out of range");
                break;
        }

        _uiManager.UpdatePlayerHealth(lives);

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
        _speedBurstDuration = 10;
        _uiManager.UpdatePlayerBoostSpeed(_speedBurstDuration);
    }

    public void DisableShip()
    {
        this.gameObject.GetComponentInChildren<ParticleSystem>().Play();
        _camera.ShakeCamera();
        _thruster.SetActive(false);
        _isDisabled = true;
        StartCoroutine(DisableShipTimer());
    }

    private IEnumerator DisableShipTimer()
    {
        yield return new WaitForSeconds(5f);
        _thruster.SetActive(true);
        _isDisabled = false;
        _particleSYS.SetActive(true);
        StopCoroutine(DisableShipTimer());
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

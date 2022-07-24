using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private UIManager _uiManager;
    private AudioSource _audio;

    //=====================================//
    //========= Private Variables =========//

    [SerializeField] private GameObject _laserPrefab, _tripleShotPrefab, _firePos1, _firePos2;
    [SerializeField] private GameObject[] _engineDamage;
    [SerializeField] private float _leftBounds, _rightBounds, _upperBounds, _lowerBounds;
    private float _timeSinceFired, _fireRate, _coolDown, _shotCount, _shotLimit;
    [SerializeField] private float _speed, _speedBurstDuration;
    private Vector3 _startPos;
    private bool canFire = true, _shieldEnabled = false, _tripleShotEnabled = false;

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

        _rightBounds = 9.4f;
        _upperBounds = 5.7f;
        _lowerBounds = -3.8f;
        _startPos = new Vector3(0,0,0);
        _coolDown = 10f;
        _shotLimit = 30f;
        _shotCount = 0f;
        _fireRate = 0.2f;
        lives = 3;
        _speed = 8f;

        // Set the start position and fire rate
        transform.position = new Vector3(0, 0, 0);
        _timeSinceFired = Time.time + _fireRate;
       
       _engineDamage[0].SetActive(false);
       _engineDamage[1].SetActive(false);

    }

 
    void Update()
    {
        PlayerMovement ();
        FireLaser();
        SpeedBurst(12f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "EnemyFire")
        {
            Damage();
            Destroy(other.gameObject);
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


    IEnumerator CoolDownTimer () {

        yield return new WaitForSecondsRealtime(_coolDown);
        _shotCount = 0;
        StopCoroutine(CoolDownTimer());

    }


    // ==================================== //
    // ============== Health ============== //

    public void Damage()
    {
        if (_shieldEnabled)
        {
            transform.GetChild(3).gameObject.SetActive(false);
            _shieldEnabled = false;
        }else
        {
            lives--;
            _uiManager.UpdatePlayerHealth(lives);
            
            if(lives == 2)
            {
                _engineDamage[0].SetActive(true);
            } else if (lives == 1)
            {
                _engineDamage[1].SetActive(true);
            }
        }

        
        if (lives <= 0) 
        {
            _gameManager.GameOver();
            this.gameObject.SetActive(false);
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

    public void EnableSpeedBoost () 
    {
        _speedBurstDuration = 10;
        _uiManager.UpdatePlayerBoostSpeed(_speedBurstDuration);
    }

    public void PowerShields()
    {
        _shieldEnabled = true;
        transform.GetChild(3).gameObject.SetActive(true);
    }

}

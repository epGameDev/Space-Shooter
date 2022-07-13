using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private UIManager _uiManager;

    //=====================================//
    //========= Private Variables =========//

    [SerializeField] private float _leftBounds, _rightBounds, _upperBounds, _lowerBounds;
    private float _coolDown, _shotCount, _shotLimit, _speed;
    [SerializeField] private float _timeSinceFired, _fireRate, _attackPower;
    [SerializeField] private GameObject _laserPrefab, _tripleShotPrefab, _firePos1, _firePos2;
    public int lives { get; private set; }
    [SerializeField] private bool _tripleShotEnabled = false;
    private bool canFire = true, _shieldEnabled = false;


    void Start()
    {
        // Initialize Values
        _gameManager.GetComponent<GameManager>();
        _uiManager.GetComponent<UIManager>();
        _leftBounds = -9.4f;
        _rightBounds = 9.4f;
        _upperBounds = 5.7f;
        _lowerBounds = -3.8f;
        _coolDown = 10f;
        _shotLimit = 30f;
        _shotCount = 0f;
        _fireRate = 0.2f;
        lives = 3;
        _speed = 8f;

        // Set the start position and fire rate
        transform.position = new Vector3(0, 0, 0);
        _timeSinceFired = Time.time + _fireRate;
       

    }

 
    void Update()
    {
        PlayerMovement ();
        FireLaser();
    }


    //==================================//
    //========= Custom Methods =========//

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
            Instantiate(_laserPrefab, _firePos1.transform.position, Quaternion.identity);

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
            transform.GetChild(0).gameObject.SetActive(false);
            _shieldEnabled = false;
        }else
        {
            lives--;
            _uiManager.UpdatePlayerHealth(lives);
            
        }

        
        if (lives <= 0) 
        {
            _gameManager.GameOver();
            Destroy(this.gameObject);
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
        _speed = 12f;
        StartCoroutine(DisableSpeedBoost());
    }

    private IEnumerator DisableSpeedBoost ()
    {
        yield return new WaitForSeconds(8f);
        _speed = 8f;
        StopCoroutine(DisableSpeedBoost());
    }

    public void PowerShields()
    {
        _shieldEnabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

}

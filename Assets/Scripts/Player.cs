using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    //=====================================//
    //========= Private Variables =========//

    [SerializeField] private float _leftBounds, _rightBounds, _upperBounds, _lowerBounds;
    [SerializeField] private float _coolDown, _shotCount, _shotLimit;
    [SerializeField] private float _timeSinceFired, _fireRate;
    [SerializeField] private GameObject _laser, _firePos1;
    private bool canFire = true;

    //====================================//
    //========= Public Variables =========//

    public float speed = 8f;


    void Start()
    {
        // Initialize Values
        _leftBounds = -9.1f;
        _rightBounds = 9.1f;
        _upperBounds = 5.9f;
        _lowerBounds = -4f;
        _coolDown = 10f;
        _shotLimit = 30f;
        _shotCount = 0f;
        _fireRate = 0.5f;
        _timeSinceFired = Time.time + _fireRate;

        // Set the start position.
        transform.position = new Vector3(0, 0, 0);
    }

 
    void Update()
    {
        PlayerMovement ();
        FireLaser();
    }


    //==================================//
    //========= Custom Methods =========//

    void PlayerMovement () 
    {
        // User Controls => GetAxis for smooth movement => GetAxisRaw for arcade movment.
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        // Player Movement
        transform.Translate( (new Vector3(horizontalInput, verticalInput, 0).normalized * speed ) * Time.deltaTime);

        // Player Restrictions
        Vector3 playerPos = transform.position;
        transform.position = new Vector3 (Mathf.Clamp(playerPos.x, _leftBounds, _rightBounds), Mathf.Clamp(playerPos.y, _lowerBounds, _upperBounds), 0);

    }

    void FireLaser ()
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
            Debug.Log("fire!");
            Instantiate(_laser, _firePos1.transform.position, Quaternion.identity);
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
        StopAllCoroutines();

    }
}

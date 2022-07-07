using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private float _speed;


    void Start()
    {
        _speed = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (transform.position.y < -4f) {
            SelfDestruct();
        }
    }

    private void OnTriggerEnter2D (Collider2D other) 
    {
        if(other.transform.tag == "Player") 
        {

            Debug.Log(other.transform.tag);
            other.transform.GetComponent<Player>().EnableTrippleShot();
            SelfDestruct();
        }
    }

    private void Movement ()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void SelfDestruct () 
    {
        Destroy(this.gameObject);
    }
}

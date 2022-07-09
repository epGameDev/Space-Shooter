using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerUpID;



    // Update is called once per frame
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
            if (player == null) 
            {
                Debug.Log("No Player Script Found");
            } else
            {
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

                    default:
                        Debug.Log("No Power Ups Were Found");
                    break;
                }
            }

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

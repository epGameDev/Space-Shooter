using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float leftBounds, rightBounds, upperBounds, lowerBounds;
    [SerializeField] private GameObject laser;

    // Public Variables
    public float speed = 8f;

    void Start()
    {
        leftBounds = -9.1f;
        rightBounds = 9.1f;
        upperBounds = 5.9f;
        lowerBounds = -4f;

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
        transform.position = new Vector3 (Mathf.Clamp(playerPos.x, leftBounds, rightBounds), Mathf.Clamp(playerPos.y, lowerBounds, upperBounds), 0);
    }

    void FireLaser ()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(laser, this.transform.position, Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float leftBounds, rightBounds, upperBounds, lowerBounds;

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

    // Update is called once per frame
    void Update()
    {
        playerMovement ();

    }

    void playerMovement () 
    {
        // User Controls
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Player Restrictions
        Vector3 playerPos = transform.position;
        transform.position = new Vector3 (Mathf.Clamp(playerPos.x, leftBounds, rightBounds), Mathf.Clamp(playerPos.y, lowerBounds, upperBounds), 0);

        // Player Movement
        transform.Translate( (new Vector3(horizontalInput, verticalInput, 0).normalized * speed ) * Time.deltaTime);
    }
}

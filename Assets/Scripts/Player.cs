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
        leftBounds = -10;
        rightBounds = 10;
        upperBounds = 5.9f;
        lowerBounds = -4;

        // Set the start position.
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 playerPos = transform.position;
        transform.position = new Vector3 (Mathf.Clamp(playerPos.x, leftBounds, rightBounds), Mathf.Clamp(playerPos.y, lowerBounds, upperBounds), 0);

        transform.Translate((new Vector3(horizontalInput, verticalInput, 0).normalized * speed) * Time.deltaTime);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Public Variables
    public float speed = 10f;

    void Start()
    {
        // Set the start position.
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // transform.Translate( ((Vector3.right * horizontalInput) * speed) * Time.deltaTime ); //// Old way
        // transform.Translate( ((Vector3.forward * verticalInput) * speed) * Time.deltaTime ); //// Old way

        // More optimal way.
        transform.Translate((new Vector3(horizontalInput, verticalInput, 0) * speed) * Time.deltaTime);
    }
}

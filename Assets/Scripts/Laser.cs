using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float laserSpeed;

    // Start is called before the first frame update
    void Start()
    {
        laserSpeed = 8;
    }

    // Update is called once per frame
    void Update()
    {
        LaserMovement();

        if (transform.position.y > 6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    void LaserMovement()
    {
        transform.Translate((Vector3.up * laserSpeed) * Time.deltaTime);
    }
}

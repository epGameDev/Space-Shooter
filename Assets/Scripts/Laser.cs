using UnityEngine;


public class Laser : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float laserSpeed = 10f;


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

using UnityEngine;


public class Laser : MonoBehaviour
{
    // Private Variables
    [SerializeField] private float laserSpeed = 10f;
    [SerializeField] private bool isEnemy;


    void Update()
    {
        if (!isEnemy)
        {
            LaserMovement(1);
        }
        else
        {
            LaserMovement(-1);
        }


    }

    void LaserMovement(int direction)
    {
        transform.Translate((new Vector3(0, direction, 0) * laserSpeed) * Time.deltaTime);

        if (transform.position.y > 10f || transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
    }

    
}

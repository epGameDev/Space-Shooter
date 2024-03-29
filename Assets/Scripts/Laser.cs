using UnityEngine;


public class Laser : MonoBehaviour
{
    //=====================================//
    //========= Private Variables =========//

    [SerializeField] private float laserSpeed = 10f;
    [SerializeField] private bool isEnemy;

    
    
    
    //=================================//
    //========= Unity Methods =========//

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

    
    
    
    //==================================//
    //========= Laser Movement =========//

    void LaserMovement(int direction)
    {
        transform.Translate((new Vector3(0, direction, 0) * laserSpeed) * Time.deltaTime);

        if (transform.position.y > 8.5f || transform.position.y < -8.5f)
        {
            Destroy(this.gameObject);
        }
    }

    
}

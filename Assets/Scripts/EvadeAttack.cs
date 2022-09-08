using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeAttack : MonoBehaviour
{

    Enemy _parent;
    [SerializeField] float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _parent = gameObject.GetComponentInParent<Enemy>();
        _speed = 18;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // Dodge Laser
        if (other.transform.tag == "laser")
        {
            _parent.ChangeDirection();
        }

        // Aligns with powerup to shoot
        if(other.transform.tag == "PowerUp" && other.transform.position.y < 2)
        {
            _parent.transform.Translate( Vector3.MoveTowards(_parent.transform.position, other.transform.position, _speed) * Time.deltaTime);
        }
    }
}

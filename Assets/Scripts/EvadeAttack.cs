using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeAttack : MonoBehaviour
{

    Enemy _parent;

    // Start is called before the first frame update
    void Start()
    {
        _parent = gameObject.GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other2) 
    {
        if (other2.transform.tag == "laser")
        {
            _parent.ChangeDirection();
        }

        if(other2.transform.tag == "PowerUp")
        {
            _parent.EnemyFire();
        }
    }
}

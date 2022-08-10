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

        if(other2.transform.tag == "PowerUp" && other2.transform.position.y < 2)
        {
            _parent.transform.Translate( Vector3.MoveTowards(_parent.transform.position, other2.transform.position, _speed) * Time.deltaTime);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] CameraController _camera;

    private void Start() 
    {
        _camera = GameObject.Find("Main Camera").GetComponent<CameraController>();  
        if (_camera == null) { Debug.Log("Explosion:: Main Camera is null"); }
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "enemy")
        {
            other.gameObject.GetComponent<Enemy>().SelfDestruct(true);
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            _camera.ShakeCamera();
        }
    }
}

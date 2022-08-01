using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private GameObject _explosionPrefab;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.transform.tag == "enemy")
        {
            _mesh.enabled = false;
            this.GetComponent<Collider2D>().enabled = false;
            _explosionPrefab.SetActive(true);
            Destroy(this.gameObject, 3f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = _player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.lives <=0)
        {
            Debug.Log("Game Over");
        }
    }
}

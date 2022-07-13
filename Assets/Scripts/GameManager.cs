using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    [SerializeField] private Player _player;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] UIManager _uiManager;
    public bool gameOver {get; private set;}

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start()
    {
        _player = _player.GetComponent<Player>();
        _spawnManager.GetComponent<SpawnManager>();
        _uiManager.GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            _spawnManager.StopSpawning();
        }

    }

    public void GameOver () 
    {
        gameOver = true;
    }
}

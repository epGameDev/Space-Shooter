using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    SceneManager sceneManager;
    [SerializeField] UIManager _uiManager;

    //=====================================//
    //========= Private Variables =========//
    [SerializeField] private Player _player;
    [SerializeField] private SpawnManager _spawnManager;

    //====================================//
    //========= Public Variables =========//
    public bool gameOver {get; private set;}


    private void Awake() {

        if (Instance)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
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
        RestartGame();
    }

    public void GameOver () 
    {
        gameOver = true;
        _spawnManager.StopSpawning();
        _uiManager.DisplayGameOver();
    }

    public void RestartGame() 
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

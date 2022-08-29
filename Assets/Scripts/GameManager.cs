using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    //=====================================//
    //========= Private Variables =========//
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] Player _player;
    private int _nextWave;

    //====================================//
    //========= Public Variables =========//
    [SerializeField] private bool gameOver;

    private void Awake() {

        if (Instance)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
    }
    
    void Start()
    {
        _uiManager.GetComponent<UIManager>();
        _spawnManager.GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_spawnManager == null || _uiManager == null || _player == null)
        {
            Debug.LogError("GameManager::UI/SpawnManager is null");
        }
        _nextWave = 1;
    }

    // Update is called once per frame
    void Update()
    {

        if (gameOver)
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void RestartGame() 
    {
        if (Input.GetButtonDown("Start") || Input.GetKeyDown(KeyCode.R))
        {
            gameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void StartWave(string wave)
    {
        switch (wave)
        {
            case "One":
                _spawnManager.StartGame(5f, 10f, 10f, 15f);
                _uiManager.DisplayCurrentWave(wave);
                break;
            case "Two":
                _spawnManager.StartGame(4f, 10f, 8f, 10f);
                _uiManager.DisplayCurrentWave(wave);
                _player.Heal();
                break;
            case "Three":
                _spawnManager.StartGame(4f, 5f, 7f, 10f);
                _uiManager.DisplayCurrentWave(wave);
                _player.Heal();
                break;
            case "Four":
                _spawnManager.StartGame(2f, 4f, 6f, 8f);
                _uiManager.DisplayCurrentWave(wave);
                _player.Heal();
                break;
            case "Five":
                _spawnManager.StartGame(2f, 3f, 5f, 6f);
                _uiManager.DisplayCurrentWave(wave);
                _player.Heal();
                break;
            default:
                Debug.LogError("GameManager:: Error on StartGame switch.");
                break;
        }
    }

    public void SetWave (int playerPoints)
    {

        if(playerPoints < 150) 
        { 
            if (_nextWave == 1) 
            {   
                StartWave("One"); 
                _nextWave++; // Next wave is 2
            }
        }
        else if (playerPoints >= 150 && playerPoints < 300) 
        { 
            if (_nextWave == 2) 
            {
                StartWave("Two"); 
                _nextWave++; // Next wave is 3
            }    
        }
        else if(playerPoints >= 300 && playerPoints < 750) 
        { 
            if (_nextWave == 3) 
            {
                StartWave("Three"); 
                _nextWave++; // Next wave is 4
            }
        }
        else if(playerPoints >= 750 && playerPoints < 1000)
        {
            if (_nextWave == 4) 
            {
                StartWave("Four"); 
                _nextWave++; // Next wave is 5
            }
        }
        else if(playerPoints >= 1000 && playerPoints < 1500)
        {
            if (_nextWave == 5) 
            {
                StartWave("Five"); 
                _nextWave++; // Next wave is 6 = Boss Battle
            }
        }
    }


    public void GameOver () 
    {
        gameOver = true;
        _spawnManager.StopSpawning();
        _uiManager.DisplayGameOver();
    }
}

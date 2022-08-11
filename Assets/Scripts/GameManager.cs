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
        if(_spawnManager == null || _uiManager == null)
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

    public void GameOver () 
    {
        gameOver = true;
        _spawnManager.StopSpawning();
        _uiManager.DisplayGameOver();
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
                _spawnManager.StartGame(4f, 6f, 10f, 20f);
                _uiManager.DisplayCurrentWave(true, wave);
                break;
            case "Two":
                _spawnManager.StartGame(3f, 5f, 10f, 15f);
                _uiManager.DisplayCurrentWave(true, wave);
                break;
            case "Three":
                _spawnManager.StartGame(3f, 5f, 10f, 13f);
                _uiManager.DisplayCurrentWave(true, wave);
                break;
            case "Four":
                _spawnManager.StartGame(2f, 4f, 7f, 11f);
                _uiManager.DisplayCurrentWave(true, wave);
                break;
            default:
                Debug.LogError("GameManager:: Error on StartGame switch.");
                break;
        }
    }

    public void SetWave (int playerPoints)
    {

        if(playerPoints < 50) 
        { 
            if (_nextWave == 1) 
            {   
                StartWave("One"); 
                _nextWave++; // Next wave is 2
            }
        }
        if (playerPoints > 50 && playerPoints < 300) 
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
        if(playerPoints >= 1000 && playerPoints < 1500)
        {
            if (_nextWave == 5) 
            {
                StartWave("Five"); 
                _nextWave++; // Next wave is 6 = Boss Battle
            }
        }
    }
}

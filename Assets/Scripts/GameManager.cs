using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] Player _player;
    [SerializeField] BossManager _boss;




    //=====================================//
    //========= Private Variables =========//

    private int _nextWave;
    private int _playerScore;
    [SerializeField] private bool gameOver;

    
    
    
    //=================================//
    //========= Unity Methods =========//
    
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
        if (_uiManager == null) { Debug.Log("GameManager:: _uiManager is null"); }
        
        _spawnManager.GetComponent<SpawnManager>();
        if (_spawnManager == null) { Debug.Log("GameManager:: _spawnManager is null"); }
        
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) { Debug.Log("GameManager:: _player is null"); }

        _boss = _boss.GetComponent<BossManager>();
        if (_boss == null) { Debug.Log("GameManager:: _boss is null"); }
        
        _nextWave = 1;
    }


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

    
    
    
    //====================================//
    //========= Wave Stage Logic =========//

    public void StartWave(string wave)
    {
        switch (wave)
        {
            case "One":
                _spawnManager.StartGame(4f, 7f, 10f, 15f);
                _uiManager.DisplayCurrentWave(wave);
                break;
            case "Two":
                _spawnManager.StartGame(4f, 5f, 8f, 10f);
                _uiManager.DisplayCurrentWave(wave);
                _player.Heal();
                break;
            case "Three":
                _spawnManager.StartGame(3f, 5f, 7f, 10f);
                _uiManager.DisplayCurrentWave(wave);
                _player.Heal();
                break;
            case "Four":
                _spawnManager.StartGame(2f, 4f, 5f, 8f);
                _uiManager.DisplayCurrentWave(wave);
                _player.Heal();
                break;
            case "Five":
                _spawnManager.StartGame(2f, 3f, 2f, 4f);
                _uiManager.DisplayCurrentWave(wave);
                _player.Heal();
                break;
            case "Boss Battle":
                Instantiate(_boss, new Vector3(0, -12, 0), Quaternion.identity);
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
        else if(playerPoints >= 300 && playerPoints < 600) 
        { 
            if (_nextWave == 3) 
            {
                StartWave("Three"); 
                _nextWave++; // Next wave is 4
            }
        }
        else if(playerPoints >= 600 && playerPoints < 1200)
        {
            if (_nextWave == 4) 
            {
                StartWave("Four"); 
                _nextWave++; // Next wave is 5
            }
        }
        else if(playerPoints >= 1200 && playerPoints < 1500)
        {
            if (_nextWave == 5) 
            {
                StartWave("Five"); 
                _nextWave++; // Next wave is 6 = Boss Battle
            }
        }
        else if (playerPoints >= 1500)
        {   
            if ( _nextWave == 6 ) 
            {
                StartWave("Boss Battle");
                _nextWave++;
            }
        }

        
    }

    
    
    
    //====================================//
    //========= Game Controllers =========//

    public void GameOver (bool playerWin = false) 
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
            // Loads the same scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

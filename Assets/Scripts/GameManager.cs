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
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            RestartGame();
        }

        if(_uiManager == null){
            Debug.Log("GameManager: _uiManager is null");
        }
        if(_spawnManager == null)
        {
            Debug.Log("GameManager: _spawnManager is null");
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
}

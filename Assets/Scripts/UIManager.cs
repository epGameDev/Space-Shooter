using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TMP_Text _scoreText, _gameOverText, _restartGameText;
    [SerializeField] private GameObject _livesUI;
    [SerializeField] private Sprite[] _playerHealthSprites;

    private int _totalScore;

    private void Awake() {
        Instance = this;
    }

    void Start()
    {
        _gameManager = GameManager.Instance;
        
        if (_gameManager == null){
            Debug.LogError("UIManager::GameManager is null");
        }

        if(_gameOverText != null || _restartGameText != null)
        {
            _gameOverText.gameObject.SetActive(false);
            _restartGameText.gameObject.SetActive(false);
        } else{
            Debug.LogError("UIManager::Game Over objects are null");
        }

        _totalScore = 0;
        _scoreText.text = "Score: " + _totalScore;
    }


    // =================================+====== //
    // ============== UI Updates ============== //

    public void GetPlayerScore (int _score) 
    {
        _totalScore += _score;
        _scoreText.text = "Score: " + _totalScore;
    }

    public void UpdatePlayerHealth (int lives)
    {
        if (_livesUI != null)
        {
            _livesUI.GetComponent<Image>().sprite = _playerHealthSprites[lives];
        }else{
            Debug.LogError("UIManager::GameObject _livesUI is null");
        }
    }


    // ==============================++========= //
    // ============== Game Status ============== //

    public void DisplayGameOver ()
    {
        _livesUI.GetComponent<Image>().sprite = _playerHealthSprites[0];
        _gameOverText.gameObject.SetActive(true);
        _restartGameText.gameObject.SetActive(true);
    }

}

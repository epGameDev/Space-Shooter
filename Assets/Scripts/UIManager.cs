using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}
    [SerializeField] private Slider _speedBoostLimitUI, _shotLimitUI;

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TMP_Text _scoreText, _gameOverText, _restartGameText, _overheatWarningText;
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

        if(_gameOverText != null && _restartGameText != null && _overheatWarningText != null)
        {
            _gameOverText.gameObject.SetActive(false);
            _restartGameText.gameObject.SetActive(false);
            _overheatWarningText.gameObject.SetActive(false);
        } else{
            Debug.LogError("UIManager::Text objects are null");
        }

        _totalScore = 0;
        _scoreText.text = "Score: " + _totalScore;
        UpdatePlayerBoostSpeed(0);
        UpdatePlayerShotLimit(false, 0);
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

    public void UpdatePlayerBoostSpeed(float currentBoostSpeed) 
    {
        if (_speedBoostLimitUI != null)
        {
            _speedBoostLimitUI.value = currentBoostSpeed;
        }
    }
    
    public void UpdatePlayerShotLimit(bool isOverHeating, float shotCount)
    {
        _shotLimitUI.value = shotCount;

        if (isOverHeating)
        {
            _overheatWarningText.gameObject.SetActive(true);
        }
        else
        {
            _overheatWarningText.gameObject.SetActive(false);
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

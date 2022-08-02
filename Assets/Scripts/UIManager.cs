using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}
    [SerializeField] private Slider _speedBoostLimitUI, _shotLimitUI;

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TMP_Text _gameOverText, _restartGameText, _overheatWarningText;
    [SerializeField] Text _playerLivesUI, _scoreText;
    [SerializeField] private GameObject _livesUI;
    [SerializeField] Text[] _ammoCount;


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
        _scoreText.text = _totalScore.ToString();
        UpdatePlayerBoostSpeed(0);
        UpdatePlayerShotLimit(false, 0);
        UpdatePlayerHealth(3);
    }


    // =================================+====== //
    // ============== UI Updates ============== //

    public void GetPlayerScore (int _score) 
    {
        _totalScore += _score;
        _scoreText.text = _totalScore.ToString();
    }

    public void UpdatePlayerHealth (int lives)
    {
        switch (lives)
        {
            case 0:
                _livesUI.GetComponent<Image>().fillAmount = 0;
                break;
            case 1:
                _livesUI.GetComponent<Image>().fillAmount = 0.33f;
                break;
            case 2:
                _livesUI.GetComponent<Image>().fillAmount = 0.66f;
                break;
            case 3:
                _livesUI.GetComponent<Image>().fillAmount = 1f;
                break;
            default:
            Debug.LogError("UIManager::GameObject _livesUI is null");
            break;
        }

        _playerLivesUI.text = lives.ToString();
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

    public void UpdateAmmoCount(int ammoID, int ammoAount, int maxAmmo)
    {
        _ammoCount[ammoID].text = (ammoAount.ToString() + " / " + maxAmmo);
    }


    // ==============================++========= //
    // ============== Game Status ============== //

    public void DisplayGameOver ()
    {
        // _livesUI.GetComponent<Image>().fillAmount = 0;
        UpdatePlayerHealth(0);
        _gameOverText.gameObject.SetActive(true);
        _restartGameText.gameObject.SetActive(true);
    }

}

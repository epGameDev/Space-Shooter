using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}
    [SerializeField] private GameManager _gameManager;

    
    
    
    //=====================================//
    //========= Private Variables =========//

    [SerializeField] private Slider _speedBoostLimitUI, _shotLimitUI, _bossHealthUI, _bossDamageUI;
    [SerializeField] private GameObject _livesUI, _bossHealthTxtPos;
    [SerializeField] private TMP_Text _gameOverText, _restartGameText, _overheatWarningText, _waveText, _healthStatusText;
    [SerializeField] Text _playerLivesUI, _scoreText;
    [SerializeField] Text _ammoCountText;
    [SerializeField] private float  _healthShrinkTime, _healthShrinkRate;
    [SerializeField] private int _totalScore;

    
    
    
    //=================================//
    //========= Unity Methods =========//

    private void Awake() 
    {
        Instance = this;
    }

    void Start()
    {
        _gameManager = GameManager.Instance;
        if (_gameManager == null) { Debug.Log("UIManager:: _gameManager is null"); }

        if (_gameOverText != null) { _gameOverText.gameObject.SetActive(false); }
        if (_restartGameText != null) { _restartGameText.gameObject.SetActive(false); }
        if (_overheatWarningText != null) { _overheatWarningText.gameObject.SetActive(false); }
        if (_waveText != null) { _waveText.gameObject.SetActive(false); }

        _healthShrinkTime = 0;
        _healthShrinkRate = 1.5f;
        _totalScore = 0;
        _scoreText.text = _totalScore.ToString();

        UpdatePlayerBoostSpeed(0);
        UpdatePlayerShotLimit(false, 0);
        UpdatePlayerHealth(3);
    }




    // =================================+====== //
    // ============== UI Updates ============== //

    public void DisplayPlayerScore (int _score) 
    {
        _totalScore += _score;
        _scoreText.text = _totalScore.ToString();
        _gameManager.SetWave(_totalScore); 
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
        _ammoCountText.text = (ammoAount.ToString() + " / " + maxAmmo);
    }

    
    
    
    //==============================//
    //========= Boss Logic =========//

    public void UpdateBossHealth(float health, bool isBossBattle)
    {
        if (isBossBattle)
        {
            _bossHealthUI.gameObject.SetActive(true);
        }
        else
        {
            _bossHealthUI.gameObject.SetActive(false);
        }
        
        _bossHealthUI.value = health;
    }


    public void BossHealthTextUpdate(float healthIncrease)
    {
        _healthStatusText.text = "+ " + healthIncrease.ToString();
        TMP_Text text = Instantiate(_healthStatusText, _bossHealthTxtPos.transform.position, Quaternion.identity);

        if (_bossHealthTxtPos != null)
        {
            text.transform.SetParent(_bossHealthTxtPos.transform);
            text.transform.localScale = new Vector3(1, 1, 1); // ! Bug Report: [X] Issue with scale on instantiation;
        }

        Destroy(text.gameObject, 3);
    }


    public void ShowUIDamage()
    {

        if (_bossHealthUI.value < _bossDamageUI.value && _bossDamageUI.gameObject.activeInHierarchy)
        {
            _bossDamageUI.value = Mathf.SmoothDamp(_bossDamageUI.value, _bossHealthUI.value, ref _healthShrinkTime, _healthShrinkRate);
        }
    }




    // ==============================++========= //
    // ============== Game Status ============== //

    public void DisplayGameOver (string displayText = "Game Over")
    {
        UpdatePlayerHealth(0);
        _gameOverText.text = displayText;
        _gameOverText.gameObject.SetActive(true);
        _restartGameText.gameObject.SetActive(true);
    }


    public void DisplayCurrentWave(string currentWave)
    {
        _waveText.gameObject.SetActive(true);
        _waveText.text = "Wave " + currentWave;

        StartCoroutine(DisplayForSetTime());
    }


    private IEnumerator DisplayForSetTime()
    {
        yield return new WaitForSeconds(10f);
        _waveText.gameObject.SetActive(false);
    }

}

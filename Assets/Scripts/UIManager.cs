using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{


    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TMP_Text _scoreText, _gameOverText, _restartGameText;
    [SerializeField] private GameObject _livesUI;
    [SerializeField] private Sprite[] _playerHealthSprites;

    private int _totalScore;

    private void Awake() {
        
    }

    void Start()
    {
        _gameManager = GameManager.Instance;
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);
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
        _livesUI.GetComponent<Image>().sprite = _playerHealthSprites[lives];
    }


    // ==============================++========= //
    // ============== Game Status ============== //

    public void DisplayGameOver ()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartGameText.gameObject.SetActive(true);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance {get; private set;}

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private GameObject _livesUI;
    [SerializeField] private Sprite[] _playerHealthSprites;

    private int _totalScore;

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        _gameManager.GetComponent<GameManager>();
        _totalScore = 0;
        _scoreText.text = "Score: " + _totalScore;
    }


    public void GetPlayerScore (int _score) 
    {
        if (!_gameManager.gameOver)
        {
            _totalScore += _score;
            _scoreText.text = "Score: " + _totalScore;
        }
    }

    public void UpdatePlayerHealth (int lives)
    {
        _livesUI.GetComponent<Image>().sprite = _playerHealthSprites[lives];
    }
}

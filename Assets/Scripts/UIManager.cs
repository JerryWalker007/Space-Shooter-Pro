using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    //handle to Text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _gameRestartText;
    [SerializeField]
    private Text _ammoCountText;
    [SerializeField]
    private Text _outOfAmmoText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoCountText.text = "Ammo: " + 15;
        _outOfAmmoText.gameObject.SetActive(false);
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void StopOutOfAmmoDisplay()
    {
        _outOfAmmoText.gameObject.SetActive(false);
    }

    public void UpdateAmmo(int playerAmmo)
    {
        _ammoCountText.text = "Ammo: " + playerAmmo.ToString();

        if (playerAmmo == 0)
        {
            _outOfAmmoText.gameObject.SetActive(true);
            StartCoroutine(OutOfAmmoFlickerRoutine());
        }
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }

        void GameOverSequence()
        {
            _gameManager.GameOver();
            _gameOverText.gameObject.SetActive(true);
            _gameRestartText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlickerRoutine());
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator OutOfAmmoFlickerRoutine()
    {
        while(true)
        {
            _outOfAmmoText.text = "OUT OF AMMO";
            yield return new WaitForSeconds(0.5f);
            _outOfAmmoText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}

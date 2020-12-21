using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private float _speedBooters = 1;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _heatSeekPrefab;
    [SerializeField]
    private float _offsetLaser = 1.05f;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    [SerializeField]
    private bool _isHeatSeekActive = false;

    [SerializeField]
    private GameObject _shieldVisualizer;
    private int _shieldStrength = 3;
    private SpriteRenderer _shieldSpriteRenderer;

    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private int _score;

    [SerializeField]
    private int _ammoCount = 15;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _shieldSpriteRenderer = _shieldVisualizer.GetComponentInChildren<SpriteRenderer>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        SpeedBoosters();

        FireWeapons();
        
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * _speedBooters * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0),0);

        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }
    }

    void FireWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_ammoCount > 0)
            {
                --_ammoCount;
                FireLaser();
                _uiManager.UpdateAmmo(_ammoCount);
            }

        }
    }

    void SpeedBoosters()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speedBooters = 3;
        }
        else
        {
            _speedBooters = 1;
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if(_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else if (_isHeatSeekActive == true)
        {
            Instantiate(_heatSeekPrefab, transform.position + new Vector3(0, 2.0f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _offsetLaser, 0), Quaternion.identity);
        }

        _audioSource.Play();
        
    }

    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _shieldStrength--;

            if (_shieldStrength > 0)
            {
                ChangeShieldColor();
            }
            else
            {
                _isShieldActive = false;
                _shieldSpriteRenderer.color = Color.white;
                _shieldVisualizer.SetActive(false);
                _shieldStrength = 3;
            }
           
            return;
        }

        _lives -= 1;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void HeatSeekingLaserActive()
    {
        _isHeatSeekActive = true;
        StartCoroutine(HeatSeekPowerDownRoutin());
    }
    IEnumerator HeatSeekPowerDownRoutin()
    {
        yield return new WaitForSeconds(5.0f);
        _isHeatSeekActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void ChangeShieldColor()
    { 

        if (_shieldStrength == 2)
        {
            _shieldSpriteRenderer.color = Color.green;
          
        }
        else
        {
            _shieldSpriteRenderer.color = Color.red;

        }
    }

    public void AmmoRecharge()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount);
        _uiManager.StopOutOfAmmoDisplay();
        _uiManager.StopAllCoroutines();
    }

    public void ExtraLife()
    {
        if (_lives >= 3)
        {
            //do nothing
        }
        else
        {
            _lives++;
        }
        
        if (_lives == 3 )
        {
            _rightEngine.SetActive(false);
        }
        else if (_lives == 2)
        {
            _leftEngine.SetActive(false);
        }

        _uiManager.UpdateLives(_lives);
    }

}

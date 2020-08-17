using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedMultiplyer = 2f;
    [SerializeField] private float _speedBoost = 8f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private GameObject _lazerPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private bool _isTripleShotEnabled = false;
    [SerializeField] private bool _isSpeedBoostActive = false;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _rightEngine, _leftEngine;
    [SerializeField] private AudioClip _lazerAudioClip;
    private AudioSource _lazerAudio;
    private int _score = 0;
    private float _canFire = -1f;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        // set starting position using current position
        transform.position = new Vector3(-0.15f, -2.5f, 0);

        _lazerAudio = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_lazerAudio == null)
        {
            Debug.LogError("Player::Audio Source is null");
        }
        else
        {
            _lazerAudio.clip = _lazerAudioClip;
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn_Manager is null");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Fire();
        }

    }

    void Fire()
    {
        // position of lazer pellet
        Vector3 offsetPos = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        Vector3 offsetPosTriple = new Vector3(transform.position.x, transform.position.y + 1.05f, transform.position.z);

        _canFire = Time.time + _fireRate;

        // if triple_shot_active true, fire prefab
        if (_isTripleShotEnabled == true)
        {
            Instantiate(_tripleShotPrefab, offsetPosTriple, Quaternion.identity);
        }
        else
        {
            Instantiate(_lazerPrefab, offsetPos, Quaternion.identity);
        }

        // Play the lazer audio clip
        _lazerAudio.Play();
    }
    void CalculateMovement()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        float xPos = transform.position.x;
        float yPos = transform.position.y;
        float zPos = transform.position.z;
        float xMax = 11.3f;
        float xMin = -11.3f;
        float yMax = 0;
        float yMin = -3.9f;

        // move by speed in meters per sec to the right or left
        // transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * _speed);

        // move by speed in meters per sec up and down
        // transform.Translate(Vector3.up * verticalInput * Time.deltaTime * _speed);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(direction * (_speedBoost * _speedMultiplyer) * Time.deltaTime);
        }
        else
        {
            // more optimized solution because only 1 new Vector3 instead of 2
            transform.Translate(direction * (_speed * _speedMultiplyer) * Time.deltaTime);
        }
        // alt method to handle y axis more efficiently
        // transform.position = new Vector3(x_pos, Mathf.Clamp(yPos, -3.8f, 0), 0);

        if (xPos < xMin)
        {
            transform.position = new Vector3(xMax, yPos, zPos);
        }
        else if (xPos > xMax)
        {
            transform.position = new Vector3(xMin, yPos, zPos);
        }

        if (yPos > yMax)
        {
            transform.position = new Vector3(xPos, yMax, zPos);
        }
        else if (yPos < yMin)
        {
            transform.position = new Vector3(xPos, yMin, zPos);
        }
    }
    public void Damage()
    {
        if (_isShieldActive == true)
        {
            _shieldVisualizer.SetActive(false);
            _isShieldActive = false;
            return;
        }

        _lives -= 1;

        // if lives is 2 enable right engine
        // if lives is 1, enable left engine
        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }
        // updates the lives displayed in UI
        _uiManager.UpdateLives(_lives);

        // check if dead, destroy if true
        if (_lives < 1)
        {
            Destroy(this.gameObject);
            _spawnManager.onPlayerDeath();
        }
    }

    public void TrippleShotActive()
    {
        _isTripleShotEnabled = true;
        // start power down coroutine for the tripple shot
        // IEnumerator TrippleShotPowerDownRoutine
        // wait 5 secs, then set Tripple Shot to false
        StartCoroutine(TrippleShotCoolDown());
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplyer;
        StartCoroutine(SpeedBoostCoolDown());
    }

    IEnumerator SpeedBoostCoolDown()
    {
        yield return new WaitForSeconds(5f);

        _isSpeedBoostActive = false;
        _speed /= _speedMultiplyer;
    }

    IEnumerator TrippleShotCoolDown()
    {
        yield return new WaitForSeconds(5.0f);

        _isTripleShotEnabled = false;
    }

    public void ShieldActive()
    {
        _shieldVisualizer.SetActive(true);
        _isShieldActive = true;
    }

    public void AddScore()
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }
}

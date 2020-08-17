using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _speedRotation = 3.0f;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private UIManager _uiManager;
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid::SpawnManager is null");
        }
        if (_uiManager == null)
        {
            Debug.LogError("Asteroid::UiManager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        // rotate on z-axis at 3mps
        transform.Rotate(Vector3.forward * _speedRotation * Time.deltaTime);

    }
    // check for lazer collosion
    // instantaite explosion at asteroid position
    // destroy explosion after 3 secs
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "lazer")
        {
            Destroy(other.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            _uiManager.StartGameSequence();
            Destroy(this.gameObject, 0.5f);
        }
    }
}

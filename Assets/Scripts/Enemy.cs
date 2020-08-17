using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;

    // handle to animator component
    private Animator _animator;
    private Player _player;
    [SerializeField] private AudioClip _explosionAudioClip;
    [SerializeField] private GameObject _lazerPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;
    private AudioSource _explosionAudio;
    // Start is called before the first frame update
    void Start()
    {
        _explosionAudio = GetComponent<AudioSource>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_explosionAudio == null)
        {
            Debug.LogError("Enemy::Audio Source is null");
        }
        else
        {
            _explosionAudio.clip = _explosionAudioClip;
        }

        // null check for player
        if (_player == null)
        {
            Debug.LogError("Enemy::The player is null");
        }
        // set 
        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Enemy::Animator is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            EnemyFire();
        }
    }

    void CalculateMovement()
    {
        // move down 4 mps, off screen re-spawn at top with new random xPos
        float yPos = transform.position.y;
        float zPos = transform.position.z;
        float randomX = Random.Range(-8f, 8f);

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (yPos <= -4f)
        {
            transform.position = new Vector3(randomX, 7.0f, 0);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // if other is Player, destroy enemey
        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.Damage();
            }
            // play anim
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosionAudio.Play();
            Destroy(this.gameObject, 2.4f);
        }
        // if other is lazer, destroy enemy and lazer
        if (other.tag == "lazer")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore();
            }
            // play explosion anim
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosionAudio.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.4f);
        }
    }

    void EnemyFire()
    {
        float xPos = transform.position.x;
        float yPos = transform.position.y;
        float zPos = transform.position.z;

        // Vector3 firePos = new Vector3(xPos, yPos + -2f, zPos);

        _fireRate = Random.Range(3f, 7f);
        _canFire = Time.time + _fireRate;

        GameObject enemyLazer = Instantiate(_lazerPrefab, transform.position, Quaternion.identity);

        Lazer[] lazers = enemyLazer.GetComponentsInChildren<Lazer>();

        // lazers[0].MakeEnemyLazer();
        // lazers[1].MakeEnemyLazer();

        for (int i = 0; i < lazers.Length; i++)
        {
            lazers[i].MakeEnemyLazer();
        }
    }
}


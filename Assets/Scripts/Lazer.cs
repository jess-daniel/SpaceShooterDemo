using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    private bool _isEnemyLazer = false;

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLazer == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }

    }

    void MoveUp()
    {
        float yPos = transform.position.y;

        // translate lazer up and set speed to 8
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // destroy lazer at y >= 8
        if (yPos >= 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        float yPos = transform.position.y;

        // translate lazer up and set speed to 8
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // destroy lazer at y >= 8
        if (yPos <= -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLazer == true)
        {
            Player _player = other.GetComponent<Player>();

            if (_player == null)
            {
                Debug.LogError("Lazer::Player is null");
            }
            _player.Damage();
        }
    }

    public void MakeEnemyLazer() => _isEnemyLazer = true;
}

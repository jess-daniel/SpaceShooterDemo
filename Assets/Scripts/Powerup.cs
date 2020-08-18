using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed = 1.5f;
    // 0 = tripple
    // 1 = speed
    // 2 = shield
    [SerializeField] private int _powerupID;
    [SerializeField] private AudioClip _powerupAudioClip;

    // Update is called once per frame
    void Update()
    {
        float yPos = transform.position.y;
        // move down at a speed of 3
        // when we leave screen, destroy it
        // check for collision with player (tag)
        transform.Translate(-transform.up * _speed * Time.deltaTime);

        if (yPos <= -4.5f)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // communicate with the player script
            Player player = other.GetComponent<Player>();

            if (player != null)
                AudioSource.PlayClipAtPoint(_powerupAudioClip, transform.position);
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TrippleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        Debug.Log("Speed Power Up Collected");
                        break;
                    case 2:
                        player.ShieldActive();
                        Debug.Log("Shield upgrade collected");
                        break;
                    case 3:
                        player.ResetAmmo();
                        Debug.Log("Powerup::Ammo has been reset");
                        break;
                    default:
                        Debug.Log("Ehh, IDK");
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}

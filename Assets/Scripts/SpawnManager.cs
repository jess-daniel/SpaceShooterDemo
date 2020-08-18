using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemeyPrefab;
    [SerializeField] private GameObject _enemeyContainer;
    [SerializeField] private GameObject _trippleShotPrefab;
    [SerializeField] private GameObject _speedBoostPrefab;
    [SerializeField] private GameObject _shieldBoostPrefab;
    private bool _stopSpawning = false;

    [SerializeField] private GameObject[] powerups;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerup());
    }
    // spawn enemy every 5 secs
    // create a coroutine of type IEnumerator
    // use a while loop
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(2f);
        // create loop based on player status
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemeyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemeyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerup()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(10f);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerup = Random.Range(0, 4);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 7f));
        }
    }

    public void onPlayerDeath()
    {
        _stopSpawning = true;
    }
}

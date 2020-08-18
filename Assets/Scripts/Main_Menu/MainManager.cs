using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadGame()
    {
        SceneManager.LoadScene(1); // load game scene
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene(2); // load game instructions
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}

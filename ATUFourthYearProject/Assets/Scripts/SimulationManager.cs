using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationManager : MonoBehaviour
{
    private static SimulationManager _instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;
        Debug.Log(Application.persistentDataPath);


        string folderPath = Path.Combine(Application.persistentDataPath, "NeuralNetworks");
        Debug.Log(folderPath);

        if (!Directory.Exists(folderPath))
        {
            Debug.Log("There is no folder yet!");
            Directory.CreateDirectory(folderPath);
        }
        else
        {
            Debug.Log("There is a new folder!");
            string[] fileNames = Directory.GetFiles(folderPath);
            Debug.Log(fileNames);
            foreach (string file in fileNames)
            {
                Debug.Log(file);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public SimulationManager GetInstance()
    {
        return _instance;
    }


    public void LoadGame()
    {
        SceneManager.LoadScene("Game Scene");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

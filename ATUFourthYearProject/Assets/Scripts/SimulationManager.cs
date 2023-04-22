using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationManager : MonoBehaviour
{
    private static SimulationManager _instance;

    public int MutationChance { get; set; }
    public float mutationValue { get; set; }

    public float scaleChange { get; set; }
    public float scaleLowerBound { get; set; }
    public float scaleUpperBound { get; set; }

    public float speedChange { get; set; }
    public float speedLowerBound { get; set; }
    public float speedUpperBound { get; set; }

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

    public static SimulationManager Instance()
    {
        return _instance;
    }

    // Method for launching game scene, is added to singleton so it can be accessed wherever this singleton exists
    public void LoadGame()
    {
        SceneManager.LoadScene("Game Scene");
    }

    // Method for launching menu scene, is added to singleton so it can be accessed wherever this singleton exists
    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // Method for quitting game, is in singleton so it can be used in any scene where this singleton exists
    public void QuitGame()
    {
        Application.Quit();
    }
}

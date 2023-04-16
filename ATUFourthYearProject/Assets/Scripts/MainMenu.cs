using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void Start()
    {
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

    public void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }



    public void QuitGame()
    {
        Application.Quit();
    }
}
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationManager : MonoBehaviour
{
    private static SimulationManager _instance;

    private int _mutationChance;
    public int MutationChance
    {
        get => _mutationChance;
        set
        {
            _mutationChance = value;
            PlayerPrefs.SetInt("MutationChance", value);
        }
    }
    private float _mutationValue;
    public float MutationValue {
        get => _mutationValue;
        set {
            _mutationValue = value;
            PlayerPrefs.SetFloat("MutationValue", value);
        } 
    }
    private float _scaleChange;
    public float ScaleChange {
        get => _scaleChange;
        set
        {
            _scaleChange = value;
            PlayerPrefs.SetFloat("ScaleChange", value);
        }
    }
    private float _scaleLowerBound;
    public float ScaleLowerBound
    {
        get => _scaleLowerBound;
        set
        {
            _scaleLowerBound = value;
            PlayerPrefs.SetFloat("ScaleLowerBound", value);
        }
    }
    private float _scaleUpperBound;
    public float ScaleUpperBound
    {
        get => _scaleUpperBound;
        set
        {
            _scaleUpperBound = value;
            PlayerPrefs.SetFloat("ScaleUpperBound", value);
        }
    }
    private float _speedChange;
    public float SpeedChange
    {
        get => _speedChange;
        set
        {
            _speedChange = value;
            PlayerPrefs.SetFloat("SpeedChange", value);
        }
    }
    private float _speedLowerBound;
    public float SpeedLowerBound
    {
        get => _speedLowerBound;
        set
        {
            _speedLowerBound = value;
            PlayerPrefs.SetFloat("SpeedLowerBound", value);
        }
    }
    private float _speedUpperBound;
    public float SpeedUpperBound
    {
        get => _speedUpperBound;
        set
        {
            _speedUpperBound = value;
            PlayerPrefs.SetFloat("SpeedUpperBound", value);
        }
    }
    private int _slimesToSpawn;
    public int SlimesToSpawn
    {
        get => _slimesToSpawn;
        set
        {
            _slimesToSpawn = value;
            PlayerPrefs.SetInt("SlimesToSpawn", value);
        }
    }
    private int _foodPerInterval;
    public int FoodPerInterval
    {
        get => _foodPerInterval;
        set
        {
            _foodPerInterval = value;
            PlayerPrefs.SetInt("FoodPerInterval", value);
        }
    }
    private int _foodCap;
    public int FoodCap
    {
        get => _foodCap;
        set
        {
            _foodCap = value;
            PlayerPrefs.SetInt("FoodCap", value);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;

        LoadPrefs();
    }

    public void LoadPrefs()
    {
        _mutationChance = PlayerPrefs.GetInt("MutationChance", 20);
        _mutationValue = PlayerPrefs.GetFloat("MutationValue", 0.05f);
        _scaleChange = PlayerPrefs.GetFloat("ScaleChange", 0.1f);
        _scaleLowerBound = PlayerPrefs.GetFloat("ScaleLowerBound", 0.5f);
        _scaleUpperBound = PlayerPrefs.GetFloat("ScaleUpperBound", 2f);
        _speedChange = PlayerPrefs.GetFloat("SpeedChange", 0.1f);
        _speedLowerBound = PlayerPrefs.GetFloat("SpeedLowerBound", 0.5f);
        _speedUpperBound = PlayerPrefs.GetFloat("SpeedUpperBound", 2f);
        _slimesToSpawn = PlayerPrefs.GetInt("SlimesToSpawn", 1000);
        _foodPerInterval = PlayerPrefs.GetInt("FoodPerInterval", 100);
        _foodCap = PlayerPrefs.GetInt("FoodCap", 1000);
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

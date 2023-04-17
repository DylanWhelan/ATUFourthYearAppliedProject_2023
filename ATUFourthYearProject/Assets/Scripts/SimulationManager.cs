using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SimulationManager Instance()
    {
        return _instance;
    }
}

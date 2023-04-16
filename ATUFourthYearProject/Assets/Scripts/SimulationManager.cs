using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    private static SimulationManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SimulationManager GetInstance()
    {
        return instance;
    }
}

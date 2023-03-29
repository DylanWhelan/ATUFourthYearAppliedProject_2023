using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    private static SimulationManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        // Set singleton instance and set DontDestroyOnLoad, to ensure cross scene persistance
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public SimulationManager GetInstance()
    {
        return instance;
    }
}

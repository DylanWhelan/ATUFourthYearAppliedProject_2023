    using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    // SlimeManager is a static singleton
    static private SlimeManager _instance;
    public int countingTheSlimes;
    public List<GameObject> whatHappensToBeHere;

    // Defines the amount of slimes to spawn in a wave
    [SerializeField] private int _numToSpawn;

    // Provides the field where the _slimePrefab is set in editor
    [SerializeField] private GameObject _slimePrefab;

    // objectPool containing all of the slimes
    [SerializeField] private ObjectPool _slimePool;

    void Awake()
    {
        // Unlikely necessary but important to ensure single instance of singleton
        if (_instance != null)
        {
            Destroy(gameObject);
        }

        // Set singleton instance
        _instance = this;

        // Instantiates the ObjectPool
        _slimePool = new ObjectPool(_slimePrefab);

        // Initial spawnWave
        SpawnWave(_numToSpawn);
    }

    public void Update()
    {
        // If there are an insufficient number of slimes, spawn a new wave
        if(_slimePool.Count == 15)
        {
            SpawnWave(_numToSpawn);
        }

        // Variables for debugging in editor
        countingTheSlimes = _slimePool.Count;
        whatHappensToBeHere = _slimePool.GetPool();
    }

    // Simple code for spawning specified number of slimes
    public void SpawnWave(int countToSpawn)
    {
        for (int i = 0; i < countToSpawn; i++)
        {
            CreateSlime(i);
        }
    }


    // create slimes methods are polymorphic through function overloading
    // If a parentslime passed in below method is called
    public void CreateSlime(GameObject parentSlime)
    {
        // Generation of random coordinate near parent slime
        float xCoord = parentSlime.transform.position.x + UnityEngine.Random.Range(-2, 2);
        float zCoord = parentSlime.transform.position.z + UnityEngine.Random.Range(-2, 2);
        float orientation = UnityEngine.Random.Range(0f, 359f);

        // The slime script is stored here to avoid an excessive count of get component commands
        Slime parentSlimeScript = parentSlime.GetComponent<Slime>();

        // object is either spawned or reactivated using object pool method
        GameObject spawnedSlime = _slimePool.GetPooledObject();
        Slime spawnedSlimeScript = spawnedSlime.GetComponent<Slime>();

        spawnedSlime.name = parentSlime.name + parentSlimeScript.GetNumChildren();

        // Stats of slime are inherited from parent and slightly modified
        float scale = Mathf.Clamp(parentSlimeScript.GetScale() + UnityEngine.Random.Range(-0.01f, 0.01f), 0.5f, 2f);
        float speed = Mathf.Clamp(parentSlimeScript.GetSpeed() + UnityEngine.Random.Range(-0.01f, 0.01f), 0.5f, 2f);

        // generation = parents generation + 1
        int generation = parentSlimeScript.GetGeneration() + 1;

        // parent's slime info is saved to be passed in to save variable
        SlimeInfo slimeInfo = parentSlimeScript.GetSlimeInfo();

        // parent's neural network is saved to be passed in
        NeuralNetwork neuralNetwork = parentSlimeScript.GetNeuralNetwork();

        // As it is unwise to create instance of classes extending monobehaviours using the new command, the init command is used as a pseudo constructor after the gameobject has been instantiated
        spawnedSlimeScript.Init(scale, speed, generation, slimeInfo, neuralNetwork);


        spawnedSlime.transform.position = new Vector3(xCoord, 0.05f, zCoord);
        spawnedSlime.transform.rotation = Quaternion.Euler(0f, orientation, 0f);
    }

    public void CreateSlime(int i)
    {
        // Generation of random coordinates
        float xCoord = UnityEngine.Random.Range(-35f, 35f);
        float zCoord = UnityEngine.Random.Range(-35f, 35f);
        float orientation = UnityEngine.Random.Range(0f, 359f);

        // object is either spawned or reactivated using object pool method
        GameObject spawnedSlime = _slimePool.GetPooledObject();

        spawnedSlime.name = string.Format("Slime_{0:0000}", i);
        Slime spawnedSlimeScript = spawnedSlime.GetComponent<Slime>();

        float scale = UnityEngine.Random.Range(0.5f, 2f);
        float speed = UnityEngine.Random.Range(0.5f, 2f);

        // pseudo constructor method for game object
        spawnedSlimeScript.Init(scale, speed);

        spawnedSlime.transform.position = new Vector3(xCoord, 0.05f  , zCoord);
        spawnedSlime.transform.rotation = Quaternion.Euler(0f, orientation, 0f);
    }

    // Gets the list of active objects from the objectPool and returns them
    public List<GameObject> GetSlimeList()
    {
        return _slimePool.GetPool();
    }

    // Deactivates the passed in object by using the deactivateObject method in objectPool
    public void DeactivateSlime(GameObject objectToDeactivate)
    {
        _slimePool.DeactivateObject(objectToDeactivate);
    }

    static public SlimeManager Instance()
    {
        return _instance;
    }
}

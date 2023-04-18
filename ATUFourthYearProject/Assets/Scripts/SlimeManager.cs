using System.Collections.Generic;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    static private SlimeManager _instance;
    [SerializeField] private int _numToSpawn;
    [SerializeField] private GameObject _slimePrefab;

    [SerializeField] private ObjectPool _slimePool;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }

        // Set singleton instance and set DontDestroyOnLoad, to ensure cross scene persistance
        _instance = this;

        _slimePool = new ObjectPool(_slimePrefab);

        SpawnWave(_numToSpawn);
    }

    public void Update()
    {
        if(_slimePool.Count == 0)
        {
            SpawnWave(_numToSpawn);
        }
    }

    public void SpawnWave(int countToSpawn)
    {
        int count = 0;
        for (int i = 0; i < countToSpawn; i++)
        {
            CreateSlime(i);
            count++;
        }
        Debug.Log(count + " " + _slimePool.Count);
    }

    public void CreateSlime(GameObject parentSlime)
    {
        // Generation of random coordinate near parent slime
        float xCoord = parentSlime.transform.position.x + UnityEngine.Random.Range(-2, 2);
        float zCoord = parentSlime.transform.position.z + UnityEngine.Random.Range(-2, 2);
        float orientation = UnityEngine.Random.Range(0f, 359f);

        Slime parentSlimeScript = parentSlime.GetComponent<Slime>();

        //GameObject spawnedSlime = Instantiate(_slimeToSpawn, new Vector3(xCoord, 0.5f, zCoord), Quaternion.Euler(0f, orientation, 0f));

        GameObject spawnedSlime = _slimePool.GetPooledObject();
        Slime spawnedSlimeScript = spawnedSlime.GetComponent<Slime>();

        spawnedSlime.name = parentSlime.name + parentSlimeScript.GetNumChildren();
        float scale = Mathf.Clamp(parentSlimeScript.GetScale() + UnityEngine.Random.Range(-0.1f, 0.1f), 0.5f, 2f);
        float speed = Mathf.Clamp(parentSlimeScript.GetSpeed() + UnityEngine.Random.Range(-0.1f, 0.1f), 0.5f, 2f);
        int generation = parentSlimeScript.GetGeneration() + 1;
        SlimeInfo slimeInfo = parentSlimeScript.GetSlimeInfo();
        NeuralNetwork neuralNetwork = parentSlimeScript.GetNeuralNetwork();
        spawnedSlimeScript.Init(scale, speed, generation, slimeInfo, neuralNetwork);


        spawnedSlime.transform.position = new Vector3(xCoord, 0.01f, zCoord);
        spawnedSlime.transform.rotation = Quaternion.Euler(0f, orientation, 0f);
    }

    public void CreateSlime(int i)
    {
        // Generation of random coordinates
        float xCoord = UnityEngine.Random.Range(-35f, 35f);
        float zCoord = UnityEngine.Random.Range(-35f, 35f);
        float orientation = UnityEngine.Random.Range(0f, 359f);
        
        GameObject spawnedSlime = _slimePool.GetPooledObject();

        spawnedSlime.name = string.Format("Slime_{0:0000}", i);
        Slime spawnedSlimeScript = spawnedSlime.GetComponent<Slime>();

        float scale = (UnityEngine.Random.Range(0.5f, 2f));
        float speed = (UnityEngine.Random.Range(0.5f, 2f));

        spawnedSlimeScript.Init(scale, speed);


        spawnedSlime.transform.position = new Vector3(xCoord, 0.01f  , zCoord);
        spawnedSlime.transform.rotation = Quaternion.Euler(0f, orientation, 0f);
    }

    public List<GameObject> GetSlimeList()
    {
        return _slimePool.GetPool();
    }

    public void DeactivateSlime(GameObject objectToDeactivate)
    {
        _slimePool.DeactivateObject(objectToDeactivate);
    }

    static public SlimeManager Instance()
    {
        return _instance;
    }
}

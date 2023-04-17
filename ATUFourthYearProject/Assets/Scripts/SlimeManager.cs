using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    static private SlimeManager _instance;
    public GameObject _slimeToSpawn;
    public int _numToSpawn;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Set singleton instance and set DontDestroyOnLoad, to ensure cross scene persistance
        _instance = this;

        SpawnWave(_numToSpawn);
    }

    public void SpawnWave(int countToSpawn)
    {
        int count = 0;
        for (int i = 0; i < countToSpawn; i++)
        {
            CreateSlime(i);
            count++;
        }
    }

    public void CreateSlime(GameObject parentSlime)
    {
        float xCoord = parentSlime.transform.position.x + UnityEngine.Random.Range(-2, 2);
        float zCoord = parentSlime.transform.position.z + UnityEngine.Random.Range(-2, 2);
        float orientation = UnityEngine.Random.Range(0f, 359f);

        Slime parentSlimeScript = parentSlime.GetComponent<Slime>();

        GameObject spawnedSlime = Instantiate(_slimeToSpawn, new Vector3(xCoord, 0.5f, zCoord), Quaternion.Euler(0f, orientation, 0f));

        Slime spawnedSlimeScript = spawnedSlime.GetComponent<Slime>();

        spawnedSlime.name = parentSlime.name + parentSlimeScript.GetNumChildren();
        float scale = Mathf.Clamp(parentSlimeScript.GetScale() + UnityEngine.Random.Range(-0.1f, 0.1f), 0.5f, 2f);
        float speed = Mathf.Clamp(parentSlimeScript.GetSpeed() + UnityEngine.Random.Range(-0.1f, 0.1f), 0.5f, 2f);
        int generation = parentSlimeScript.GetGeneration() + 1;
        SlimeInfo slimeInfo = parentSlimeScript.GetSlimeInfo();
        NeuralNetwork neuralNetwork = parentSlimeScript.GetNeuralNetwork();
        spawnedSlimeScript.Init(scale, speed, generation, slimeInfo, neuralNetwork);
    }

    public void CreateSlime(int i)
    {
        float xCoord = UnityEngine.Random.Range(-35f, 35f);
        float zCoord = UnityEngine.Random.Range(-35f, 35f);
        float orientation = UnityEngine.Random.Range(0f, 359f);
        GameObject spawnedSlime = Instantiate(_slimeToSpawn, new Vector3(xCoord, 0.6f, zCoord), Quaternion.Euler(0f, orientation, 0f));
        spawnedSlime.name = string.Format("Slime_{0:0000}", i);
        Slime spawnedSlimeScript = spawnedSlime.GetComponent<Slime>();

        float scale = (UnityEngine.Random.Range(0.5f, 2f));
        float speed = (UnityEngine.Random.Range(0.5f, 2f));

        spawnedSlimeScript.Init(scale, speed);
    }

    static public SlimeManager Instance()
    {
        return _instance;
    }
}

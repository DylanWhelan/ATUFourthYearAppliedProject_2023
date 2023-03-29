using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    static private SlimeSpawner instance;
    public GameObject slimeToSpawn;

    public int numToSpawn;
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

        int count = 0;
        for (int i = 0; i < numToSpawn; i++) {
            /*float xCoord = UnityEngine.Random.Range(-35f, 35f);
            float zCoord = UnityEngine.Random.Range(-35f, 35f);
            float orientation = UnityEngine.Random.Range(0f, 359f);
            GameObject spawnedSlime = Instantiate(slimeToSpawn, new Vector3(xCoord, 1, zCoord), Quaternion.Euler(0f, orientation, 0f));
            spawnedSlime.name = string.Format("Slime_{0:0000}", i);
            spawnedSlime.GetComponent<Slime>().SetScale(UnityEngine.Random.Range(0.5f, 2f));*/
            CreateSlime(i);
            count++;
        }
        Debug.Log("Num slimes spawned: " + count);
    }

    public void CreateSlime(GameObject parentSlime) {
        float xCoord = parentSlime.transform.position.x + UnityEngine.Random.Range(-2, 2);
        float zCoord = parentSlime.transform.position.z + UnityEngine.Random.Range(-2, 2);
        float orientation = UnityEngine.Random.Range(0f, 359f);

        GameObject spawnedSlime = Instantiate(slimeToSpawn, new Vector3(xCoord, 1, zCoord), Quaternion.Euler(0f, orientation, 0f));
        spawnedSlime.name = parentSlime.name + parentSlime.GetComponent<Slime>().GetNumChildren();
        spawnedSlime.GetComponent<Slime>().SetScale(Mathf.Clamp(parentSlime.GetComponent<Slime>().GetScale() + UnityEngine.Random.Range(-0.1f, 0.1f), 0.5f, 2f));
    }

    public void CreateSlime(int i) {
            float xCoord = UnityEngine.Random.Range(-35f, 35f);
            float zCoord = UnityEngine.Random.Range(-35f, 35f);
            float orientation = UnityEngine.Random.Range(0f, 359f);
            GameObject spawnedSlime = Instantiate(slimeToSpawn, new Vector3(xCoord, 1, zCoord), Quaternion.Euler(0f, orientation, 0f));
            spawnedSlime.name = string.Format("Slime_{0:0000}", i);
            spawnedSlime.GetComponent<Slime>().SetScale(UnityEngine.Random.Range(0.5f, 2f));
    }

    static public SlimeSpawner Instance() {
        return instance;
    }
}

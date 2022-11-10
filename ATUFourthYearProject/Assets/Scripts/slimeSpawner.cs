using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeSpawner : MonoBehaviour
{
    public GameObject slimeToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 75; i++) {
            float xCoord = UnityEngine.Random.Range(-35f, 35f);
            float zCoord = UnityEngine.Random.Range(-35f, 35f);
            float orientation = UnityEngine.Random.Range(0f, 359f);
            GameObject spawnedSlime = Instantiate(slimeToSpawn, new Vector3(xCoord, 3, zCoord), Quaternion.Euler(0f, orientation, 0f));
            spawnedSlime.name = string.Format("Slime_{0:0000}", i);
            spawnedSlime.GetComponent<Slime>().SetScale(UnityEngine.Random.Range(0.5f, 2f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

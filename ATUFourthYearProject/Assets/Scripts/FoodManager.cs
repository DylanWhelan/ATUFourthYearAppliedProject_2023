using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public List<GameObject> foodList;
    List<GameObject> spawnedFoods;

    public float spawningInterval;
    float timeElapsed;

    void Start()
    {
        spawnedFoods = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= spawningInterval) {
            Debug.Log("Was here!");
            timeElapsed = 0f;
            if (spawnedFoods.Count <= 100) {
                for (int i = 0; i < 75; i++) {
                    Debug.Log(i);
                    float xCoord = UnityEngine.Random.Range(-35f, 35f);
                    float zCoord = UnityEngine.Random.Range(-35f, 35f);
                    float orientation = UnityEngine.Random.Range(0f, 359f);
                    GameObject spawnedFood = Instantiate(foodList[0], new Vector3(xCoord, 3, zCoord), Quaternion.Euler(0f, orientation, 0f));
                    spawnedFood.GetComponent<Food>().SetSaturation(25); 
                    spawnedFoods.Add(spawnedFood);
                }
            }
        }
    }
}

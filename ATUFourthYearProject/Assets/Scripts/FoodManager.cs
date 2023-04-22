using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    // Is a singleton 
    static private FoodManager _instance;

    // Serialized field where the food prefab can be set in Unity editor
    [SerializeField] private GameObject _foodPrefab;

    // caps the maximum amount of food in game
    [SerializeField] private int _foodCap;

    // Sets the maximum of food that can be spawned per interval
    [SerializeField] private int _foodPerInterval;


    // Defines amount of time before spawning waves
    [SerializeField] private float _spawningInterval;
    private float _timeElapsed;

    // An objectPool for storing food Object
    [SerializeField] private ObjectPool _foodPool;

    void Start()
    {
        // If there is another instance of this class, delete this one
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;

        // instantiates objectPool for the food prefab
        _foodPool = new ObjectPool(_foodPrefab);

        SpawnFoods();
    }

    // Update is called once per frame
    void Update()
    {
        // food is spawned on a recurring basis
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed >= _spawningInterval)
        {
            _timeElapsed = 0f;
            SpawnFoods();
        }
    }

    static public FoodManager Instance()
    {
        return _instance;
    }

    // The spawnfoods wave
    void SpawnFoods()
    {
        // the loop is ran for an amount of times indicated by foodPerInterval
        for (int i = 0; i < _foodPerInterval; i++)
        {
            // if _foodPool count is greater than food count, don't spawn any food
            if (_foodPool.Count < _foodCap)
            {
                // position of food is defined
                float xCoord = UnityEngine.Random.Range(-35f, 35f);
                float zCoord = UnityEngine.Random.Range(-35f, 35f);
                float orientation = UnityEngine.Random.Range(0f, 359f);

                // food object is returned from GetPooledObject method
                GameObject spawnedFood = _foodPool.GetPooledObject();
                spawnedFood.transform.position = new Vector3(xCoord, 0.15f, zCoord);
                spawnedFood.transform.rotation = Quaternion.Euler(0f, orientation, 0f);

                spawnedFood.GetComponent<Food>().SetSaturation(25);
            }
            
        }
    }

    // Gets the list of activeObjects from food ObjectPool
    public List<GameObject> GetFoodList()
    {
        return _foodPool.GetPool();
    }

    // Calls the deactivateObject method in the objectPool
    public void DeactivateFood(GameObject objectToDeactivate)
    {
        _foodPool.DeactivateObject(objectToDeactivate);
    }
}

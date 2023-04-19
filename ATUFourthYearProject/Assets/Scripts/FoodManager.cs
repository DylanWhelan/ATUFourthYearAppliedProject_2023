using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    static private FoodManager _instance;

    [SerializeField] private GameObject _foodPrefab;
    [SerializeField] private int _foodCap;
    [SerializeField] private int _foodPerInterval;

    [SerializeField] private float _spawningInterval;
    private float _timeElapsed;

    [SerializeField] private ObjectPool _foodPool;

    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;

        _foodPool = new ObjectPool(_foodPrefab);

        SpawnFoods();
    }

    // Update is called once per frame
    void Update()
    {
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

    void SpawnFoods()
    {
        Debug.Log(_foodPool.Count);
        for (int i = 0; i < _foodPerInterval; i++)
        {
            if (_foodPool.Count < _foodCap)
            {
                float xCoord = UnityEngine.Random.Range(-35f, 35f);
                float zCoord = UnityEngine.Random.Range(-35f, 35f);
                float orientation = UnityEngine.Random.Range(0f, 359f);

                GameObject spawnedFood = _foodPool.GetPooledObject();
                spawnedFood.transform.position = new Vector3(xCoord, 0.15f, zCoord);
                spawnedFood.transform.rotation = Quaternion.Euler(0f, orientation, 0f);

                spawnedFood.GetComponent<Food>().SetSaturation(25);
            }
            
        }
    }

    public List<GameObject> GetFoodList()
    {
        return _foodPool.GetPool();
    }

    public void DeactivateFood(GameObject objectToDeactivate)
    {
        _foodPool.DeactivateObject(objectToDeactivate);
    }
}

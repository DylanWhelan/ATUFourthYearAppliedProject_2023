using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> _activeObjectPool;
    private Queue<GameObject> _inactiveObjectPool;

    [SerializeField]private GameObject _pooledObject;

    private void Start()
    {
        _activeObjectPool = new List<GameObject>();
        _inactiveObjectPool = new Queue<GameObject>();
    }

    public void DeactivateObject(GameObject objectToDeactivate)
    {
        objectToDeactivate.SetActive(false);
        _activeObjectPool.Remove(objectToDeactivate);
        _inactiveObjectPool.Enqueue(objectToDeactivate);
    }

    public List<GameObject> GetPool()
    {
        return _activeObjectPool;
    }

    public GameObject GetPooledObject()
    {
        GameObject objectToReturn;
        if (_inactiveObjectPool.Count != 0)
        {
            objectToReturn = _inactiveObjectPool.Dequeue();
        }
        else
        {
            objectToReturn = Instantiate(_pooledObject);
        }
        objectToReturn.SetActive(true);
        _activeObjectPool.Add(objectToReturn);
        return objectToReturn;
    }

    public int Count()
    {
        return _activeObjectPool.Count;
    }
}

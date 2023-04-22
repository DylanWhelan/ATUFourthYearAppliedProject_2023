using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    // activeObjectPpol is a list as iterating through the collection without removing elements is required for checking closest items
    private List<GameObject> _activeObjectPool;

    // _inactiveObjectPool is a queue as the only purpose of the collection is to keep objects which are only to be removed from the pool
    private Queue<GameObject> _inactiveObjectPool;

    // Count is a c# property rather than a method
    public int Count { get => _activeObjectPool.Count; }

    // The gameObject that the pool is containing
    [SerializeField]private GameObject _pooledObject;


    // The constructor for the ObjectPool, contains the passed in gameObject
    public ObjectPool(GameObject pooledObject)
    {
        _activeObjectPool = new List<GameObject>();
        _inactiveObjectPool = new Queue<GameObject>();
        _pooledObject = pooledObject;
    }

    // Passed in object is deactivated, removed from the activepool and added to inactivepool
    public void DeactivateObject(GameObject objectToDeactivate)
    {
        objectToDeactivate.SetActive(false);
        _activeObjectPool.Remove(objectToDeactivate);
        _inactiveObjectPool.Enqueue(objectToDeactivate);
    }

    // Returns the activeObjectPool
    public List<GameObject> GetPool()
    {
        return _activeObjectPool;
    }
    

    // Either returns an object from the inactiveObjectPool or instantiates a new object if the inactiveObjectPool is empty
    public GameObject GetPooledObject()
    {
        // Object variable is declared
        GameObject objectToReturn;
        if (_inactiveObjectPool.Count != 0)
        {
            // if inactivePool isn't empty then take object from queue
            objectToReturn = _inactiveObjectPool.Dequeue();
            objectToReturn.SetActive(true);
        }
        else
        {
            // if pool is empty then instatiate new object
            objectToReturn = Object.Instantiate(_pooledObject);
        }
        // object is added to activeObjectPool
        _activeObjectPool.Add(objectToReturn);
        return objectToReturn;
    }
}

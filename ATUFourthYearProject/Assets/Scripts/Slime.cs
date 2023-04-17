using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private float _scale;
    private float _speed;
    private int _generation;

    private float _saturation;

    private int _numChildren;

    [SerializeField] private float[] _inputsToNeural = new float [5];
    [SerializeField] private float[] _outputsOfNeural = new float [2];

    private SlimeInfo _slimeInfo;
    private NeuralNetwork _neuralNetwork;

    private GameObject closestFood;
    // Start is called before the first frame update
    void Start()
    {
        _numChildren = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        _saturation -= 0.5f * _scale * Time.deltaTime;

        if (closestFood == null) {
            GameObject [] foods = GameObject.FindGameObjectsWithTag("Food");
            if (foods.Length != 0) {
                closestFood = NearestFood(foods);
            }
        }

        if (_saturation < 0)
        {
            Die();
        }
        else if (_saturation > 100f * _scale)
        {
            CreateChild();
        }
        // Represents slimes current hunger
        _inputsToNeural[0] = _saturation / 50f - 1f;
        // Represents slimes size
        _inputsToNeural[1] = _scale - 1f;
        if (closestFood == null) {
            // Represents saturation of food
            _inputsToNeural[2] = -1f;
            // Represents angle to food in pi radians
            _inputsToNeural[3] = -1f;
            // represents distance to food
            _inputsToNeural[4] = 1f;
        }
        else {
            _inputsToNeural[2] = 1f;
            _inputsToNeural[3] = (Mathf.Deg2Rad * Vector2.Angle(transform.forward, (Vector2) (closestFood.transform.position - transform.position))) - 1;
            _inputsToNeural[4] = Mathf.Clamp((Vector3.Distance(closestFood.transform.position, transform.position) / 25f - 1f), -1f, 1f);
        }

        try
        {
            _outputsOfNeural = _neuralNetwork.FeedForward(_inputsToNeural);
            Rotate(_outputsOfNeural[0]);
            MoveForward(_outputsOfNeural[1]);
        } catch (System.NullReferenceException)
        {
            Debug.Log("This can't" + name);
        }
        
    }

    void Rotate(float piRadian) {
        transform.Rotate(transform.up, piRadian);
    }

    void MoveForward(float movementModifier) {
        if (movementModifier < 0) return;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * _speed * movementModifier * 1 *  Time.deltaTime);
        _saturation -= 1f * _scale * _speed * Time.deltaTime;
    }

    GameObject NearestFood(GameObject [] foods)
    {
        GameObject closestFood = null;
        float closestSqrDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach(GameObject food in foods)
        {
            Vector3 directionToTarget = food.transform.position - currentPosition;
            float sqrDistanceToTarget = directionToTarget.sqrMagnitude;
            if (sqrDistanceToTarget < closestSqrDistance)
            {
                closestSqrDistance = sqrDistanceToTarget;
                closestFood = food;
            }
            
        }
        return closestFood;
    }

    void OnCollisionEnter(Collision collider) {
        if (collider.transform.name.Contains("Food")) {
            _saturation += collider.transform.GetComponent<Food>().IsEaten();
            closestFood = null;
        }
    }

    public void Init(float slimeScale, float slimeSpeed)
    {
        SetScale(slimeScale);
        _speed = slimeSpeed;
        _generation = 0;
        _slimeInfo = new SlimeInfo(name, _scale, _speed);
        _neuralNetwork = new NeuralNetwork(new int[] { 5, 4, 4, 2 });
        _numChildren = 0;
    }

    public void Init(float slimeScale, float slimeSpeed, int generation, SlimeInfo parentSlime, NeuralNetwork neuralNetwork)
    {
        SetScale(slimeScale);
        _speed = slimeSpeed;
        _generation = generation;
        _slimeInfo = new SlimeInfo(name, _scale, _speed, generation, parentSlime);
        _neuralNetwork = new NeuralNetwork(neuralNetwork);
        _numChildren = 0;
    }


    public NeuralNetwork GetNeuralNetwork()
    {
        return _neuralNetwork;
    }

    public void SetNeuralNetwork(NeuralNetwork neuralNetwork)
    {
        this._neuralNetwork = new NeuralNetwork(neuralNetwork);
    }

    public SlimeInfo GetSlimeInfo()
    {
        return _slimeInfo;
    }

    private void SetScale(float newScale)
    {
        _scale = newScale;
        _saturation = 50 * _scale;
        gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public float GetScale()
    {
        return _scale;
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public int GetGeneration()
    {
        return _generation;
    }
    public float GetSaturation()
    {
        return _saturation;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void CreateChild()
    {
        Debug.Log(name + " has eaten enough to have a child!"); 
        _saturation = 50f * _scale;
        _numChildren += 1;
        SlimeManager.Instance().CreateSlime(gameObject);
    }
    
    public int GetNumChildren()
    {
        return _numChildren;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private float _scale;
    private float _speed;
    private int _generation;


    private float _saturation;
    private float _saturationIfEaten;
    private bool _isAlive;

    private int _numChildren;

    [SerializeField] private float[] _inputsToNeural = new float[6];
    [SerializeField] private float[] _outputsOfNeural = new float[2];

    private SlimeInfo _slimeInfo;
    private NeuralNetwork _neuralNetwork;

    private GameObject closestFood;
    private GameObject closestSlime;

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            _saturation -= 0.5f * _scale * Time.deltaTime;

            if (closestFood == null)
            {
                if (FoodManager.Instance().GetFoodList().Count != 0)
                {
                    closestFood = NearestObject(FoodManager.Instance().GetFoodList());
                }
            }

            if (closestSlime == null)
            {
                if (SlimeManager.Instance().GetSlimeList().Count != 0)
                {
                    closestSlime = NearestObject(SlimeManager.Instance().GetSlimeList());
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
            if (closestFood == null)
            {
                // Represents saturation of food
                _inputsToNeural[3] = -1f;
                // Represents angle to food in pi radians
                _inputsToNeural[4] = -1f;
                // represents distance to food
                _inputsToNeural[5] = 1f;
            }
            else
            {
                _inputsToNeural[3] = 1f;
                _inputsToNeural[4] = (Mathf.Deg2Rad * Vector2.Angle(transform.forward, (Vector2)(closestFood.transform.position - transform.position))) - 1;
                _inputsToNeural[5] = Mathf.Clamp((Vector3.Distance(closestFood.transform.position, transform.position) / 25f - 1f), -1f, 1f);
            }
            _outputsOfNeural = _neuralNetwork.FeedForward(_inputsToNeural);
            Rotate(_outputsOfNeural[0]);
            MoveForward(_outputsOfNeural[1]);
        }
        else
        {
            _saturationIfEaten -= 1f * Time.deltaTime;
            if (_saturationIfEaten < 0)
            {
                SetInactive();
            }
        }
    }

    void Rotate(float piRadian)
    {
        transform.Rotate(transform.up, piRadian);
    }

    void MoveForward(float movementModifier)
    {
        if (movementModifier < 0) return;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * _speed * movementModifier * 1 * Time.deltaTime);
        _saturation -= 1f * _scale * _speed * Time.deltaTime;
    }

    GameObject NearestObject(List<GameObject> objectList)
    {
        GameObject closestObject = null;
        float closestSqrDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject objectToCheck in objectList)
        {
            Vector3 directionToTarget = objectToCheck.transform.position - currentPosition;
            float sqrDistanceToTarget = directionToTarget.sqrMagnitude;
            if (sqrDistanceToTarget < closestSqrDistance && objectToCheck != gameObject)
            {
                closestSqrDistance = sqrDistanceToTarget;
                closestObject = objectToCheck;
            }

        }
        return closestObject;
    }

    void OnCollisionEnter(Collision collider)
    {
        if (!_isAlive)
        {
            return;
        }
        if (collider.gameObject.CompareTag("Food"))
        {
            _saturation += collider.gameObject.GetComponent<Food>().IsEaten();
            closestFood = null;
        }
        else if (collider.gameObject.CompareTag("Slime"))
        {
            Slime otherSlime = collider.gameObject.GetComponent<Slime>();
            if (_scale > (otherSlime.GetScale() * 1.1f))
            {
                _saturation += collider.gameObject.GetComponent<Slime>().IsEaten();
                Debug.Log(name + " has eaten " + collider.gameObject.name);
            }
        }
    }

    public void Init(float slimeScale, float slimeSpeed)
    {
        SetScale(slimeScale);
        _speed = slimeSpeed;
        _generation = 0;
        _slimeInfo = new SlimeInfo(name, _scale, _speed);
        _neuralNetwork = new NeuralNetwork(new int[] { 6, 4, 4, 2 });
        InitCommon();
    }

    public void Init(float slimeScale, float slimeSpeed, int generation, SlimeInfo parentSlime, NeuralNetwork neuralNetwork)
    {
        SetScale(slimeScale);
        _speed = slimeSpeed;
        _generation = generation;
        _slimeInfo = new SlimeInfo(name, _scale, _speed, generation, parentSlime);
        _neuralNetwork = new NeuralNetwork(neuralNetwork);
        InitCommon();
    }

    // Things that need to be done regardless of constructor
    private void InitCommon()
    {
        _numChildren = 0;
        _isAlive = true;

        _saturationIfEaten = _saturation;
        // Setting neural inputs that persist for the life of the slime
        _inputsToNeural[1] = _scale;
        _inputsToNeural[2] = _speed;
    }


    public NeuralNetwork GetNeuralNetwork()
    {
        return _neuralNetwork;
    }

    public void SetNeuralNetwork(NeuralNetwork neuralNetwork)
    {
        _neuralNetwork = new NeuralNetwork(neuralNetwork);
    }

    public SlimeInfo GetSlimeInfo()
    {
        return _slimeInfo;
    }

    private void SetScale(float newScale)
    {
        _scale = newScale;
        _saturation = 50 * _scale;
        newScale = Mathf.Sqrt(newScale);
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

    public float GetSaturationIfEaten()
    {
        return _saturationIfEaten;
    }

    void Die()
    {
        //SlimeManager.Instance().DeactivateSlime(gameObject);
        Debug.Log(name + " would provide: " + _saturationIfEaten + " if eaten!");
        _isAlive = false;
    }

    public float IsEaten()
    {
        SetInactive();
        return _saturationIfEaten;
    }

    void SetInactive()
    {
        SlimeManager.Instance().DeactivateSlime(gameObject);
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

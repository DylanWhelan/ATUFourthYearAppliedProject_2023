using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private float _scale;
    private float _speed;
    private int _generation;

    [SerializeField] private Rigidbody rb;

    private float _saturation;
    private float _saturationIfEaten;
    private bool _isAlive;

    private int _numChildren;

    [SerializeField] private float[] _inputsToNeural = new float[6];
    [SerializeField] private float[] _outputsOfNeural = new float[2];

    private SlimeInfo _slimeInfo;
    private NeuralNetwork _neuralNetwork;

    private GameObject _closestFood;
    private GameObject _closestSlime;

    private float _foodCheckTimer;
    private float _slimeCheckTimer;

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            _saturation -= 0.5f * _scale * Time.deltaTime;

            if (_closestFood == null)
            {
                if (FoodManager.Instance().GetFoodList().Count != 0 || _foodCheckTimer > 3f)
                {
                    _closestFood = NearestObject(FoodManager.Instance().GetFoodList());
                    _foodCheckTimer = 0;
                }
            }

            if (_closestSlime == null || _slimeCheckTimer > 3f)
            {
                if (SlimeManager.Instance().GetSlimeList().Count != 0)
                {
                    _closestSlime = NearestObject(SlimeManager.Instance().GetSlimeList());
                    _slimeCheckTimer = 0;
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
            if (_closestFood == null)
            {
                // Represents saturation of food, -1 if null
                _inputsToNeural[3] = -1f;
                // Represents angle to food in pi radians, 0 if null
                _inputsToNeural[4] = 0;
                // represents distance to food, 1 if null
                _inputsToNeural[5] = 1f;
            }
            else
            {
                // Represents saturation of food, -0.5f due to following formula
                // value = (saturation / 50) - 1, so for food which has default 25 = -0.5f
                _inputsToNeural[3] = -0.5f;
                // Represents angle of food relative to slime in pi radians to fit in with scheme of inputs being -1 to 1,
                // Negative values are on the left side, positive values are on the right
                _inputsToNeural[4] = (Mathf.Deg2Rad * Vector2.Angle(transform.forward, (Vector2)(_closestFood.transform.position - transform.position))) - 1;
                // Represents distance to food from slime, with values between -1 and 1,
                // And distance greater than 25 is capped at an input value of 1
                _inputsToNeural[5] = Mathf.Clamp((Vector3.Distance(_closestFood.transform.position, transform.position) / 12.5f) - 1f, -1f, 1f);
            }
            if (_closestSlime == null)
            {
                // Represents scale of closestSlime, -1 if null
                _inputsToNeural[6] = -1f;
                // Represents speed of closestSlime, -1 if null
                _inputsToNeural[7] = -1f;
                // Represents angle to closestSlime, 0 if null
                _inputsToNeural[8] = 0f;
                // Represents distance to closestSlime,
                _inputsToNeural[9] = 1f;
                // Represents saturation value of closestSlime, -1 if null
                _inputsToNeural[10] = -1f;
                // Represents status of closestSlime, -1 if null
                _inputsToNeural[11] = -1f;
            }
            else
            {
                Slime closestSlimeScript = _closestSlime.GetComponent<Slime>();
                // Represents scale of closestSlime, values are clamped between 0.5 and 2 typically
                // So take away one to fit within established scheme for neural net inputs
                _inputsToNeural[6] = closestSlimeScript.GetScale() - 1;
                // Represents speed of closestSlime, values are clamped between 0.5 and 2 typically
                // So take away one to fit within established scheme for neural net inputs
                _inputsToNeural[7] = closestSlimeScript.GetSpeed() - 1;
                // Represents angle of closestSlime relative to slime in pi radians to fit in with scheme of inputs being -1 to 1,
                // Negative values are on the left side, positive values are on the right
                _inputsToNeural[8] = (Mathf.Deg2Rad * Vector2.Angle(transform.forward, (Vector2)(_closestSlime.transform.position - transform.position))) - 1;
                // Represents distance to closestSlime from slime, with values between -1 and 1,
                // And distance greater than 25 is capped at an input value of 1
                _inputsToNeural[9] = Mathf.Clamp((Vector3.Distance(_closestFood.transform.position, transform.position) / 12.5f) - 1f, -1f, 1f);
                // Represents saturation value of closestSlime, calculated with following formula,
                // input = (saturationIfEaten / 50) = 1
                _inputsToNeural[10] = (closestSlimeScript.GetSaturationIfEaten() / 50f) - 1;
                // Represents status of closestSlime, 0 if dead, 1 if alive
                _inputsToNeural[11] = closestSlimeScript.IsAlive() ? 1 : 0;
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
        //transform.Rotate(transform.up, piRadian);
        transform.Rotate(Vector3.up, piRadian * Mathf.Rad2Deg * Time.deltaTime);    
        Vector3 projectedForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        transform.rotation = Quaternion.LookRotation(projectedForward, Vector3.up);
    }

    void MoveForward(float movementModifier)
    {
        if (movementModifier < 0) return;
        rb.AddForce(transform.forward * _speed * movementModifier * 5 * Time.deltaTime, ForceMode.Acceleration);
        _saturation -= 0.5f * _scale * _speed * Time.deltaTime;
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
            _closestFood = null;
        }
        else if (collider.gameObject.CompareTag("Slime"))
        {
            Slime otherSlime = collider.gameObject.GetComponent<Slime>();
            if (_scale > (otherSlime.GetScale() * 1.1f))
            {
                _saturation += collider.gameObject.GetComponent<Slime>().IsEaten();
            }
        }
    }

    public void Init(float slimeScale, float slimeSpeed)
    {
        SetScale(slimeScale);
        _speed = slimeSpeed;
        _generation = 0;
        _slimeInfo = new SlimeInfo(name, _scale, _speed);
        _neuralNetwork = new NeuralNetwork(new int[] { 12, 4, 4, 2 });
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

    public bool IsAlive()
    {
        return _isAlive;
    }

    void Die()
    {
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
        _saturation = 50f * _scale;
        _numChildren += 1;
        _slimeInfo.SlimeChildren = _numChildren;
        SlimeManager.Instance().CreateSlime(gameObject);
    }

    public int GetNumChildren()
    {
        return _numChildren;
    }
}

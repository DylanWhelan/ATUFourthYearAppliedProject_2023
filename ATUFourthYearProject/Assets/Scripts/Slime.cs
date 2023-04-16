using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private float scale;
    private float speed;
    private int generation;

    private float saturation = 30f;

    private int numChildren;

    [SerializeField] private float[] inputsToNeural = new float [5];
    [SerializeField] private float[] outputsOfNeural = new float [2];

    private SlimeInfo slimeInfo;
    private NeuralNetwork neuralNetwork;

    private GameObject closestFood;
    // Start is called before the first frame update
    void Start()
    {
        numChildren = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        saturation -= 0.5f * scale * Time.deltaTime;

        if (closestFood == null) {
            GameObject [] foods = GameObject.FindGameObjectsWithTag("Food");
            if (foods.Length != 0) {
                closestFood = NearestFood(foods);
            }
        }

        /*if (closestFood != null) {
            Jump(closestFood);
        }*/

        if (saturation < 0)
        {
            Die();
        }
        else if (saturation > 100f * scale)
        {
            CreateChild();
        }
        // Represents slimes current hunger
        inputsToNeural[0] = saturation / 50f - 1f;
        // Represents slimes size
        inputsToNeural[1] = scale - 1f;
        if (closestFood == null) {
            // Represents saturation of food
            inputsToNeural[2] = -1f;
            // Represents angle to food in pi radians
            inputsToNeural[3] = -1f;
            // represents distance to food
            inputsToNeural[4] = 1f;
        }
        else {
            inputsToNeural[2] = 1f;
            inputsToNeural[3] = (Mathf.Deg2Rad * Vector2.Angle(transform.forward, (Vector2) (closestFood.transform.position - transform.position))) - 1;
            inputsToNeural[4] = Mathf.Clamp((Vector3.Distance(closestFood.transform.position, transform.position) / 25f - 1f), -1f, 1f);
        }

        try
        {
            outputsOfNeural = neuralNetwork.FeedForward(inputsToNeural);
            Rotate(outputsOfNeural[0]);
            MoveForward(outputsOfNeural[1]);
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
        rb.AddForce(transform.forward * speed * movementModifier * 1 *  Time.deltaTime);
        saturation -= 1f * scale * speed * Time.deltaTime;
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
            saturation += collider.transform.GetComponent<Food>().IsEaten();
            closestFood = null;
        }
    }

    public void init(float slimeScale, float slimeSpeed)
    {
        SetScale(slimeScale);
        this.speed = slimeSpeed;
        this.generation = 0;
        this.slimeInfo = new SlimeInfo(name, scale, speed);
        this.neuralNetwork = new NeuralNetwork(new int[] { 5, 4, 4, 2 });
    }

    public void init(float slimeScale, float slimeSpeed, int generation, SlimeInfo parentSlime, NeuralNetwork neuralNetwork)
    {
        SetScale(slimeScale);
        this.speed = slimeSpeed;
        this.generation = generation;
        this.slimeInfo = new SlimeInfo(name, scale, speed, generation, parentSlime);
        this.neuralNetwork = new NeuralNetwork(neuralNetwork);
    }


    public NeuralNetwork GetNeuralNetwork()
    {
        return neuralNetwork;
    }

    public void SetNeuralNetwork(NeuralNetwork neuralNetwork)
    {
        this.neuralNetwork = new NeuralNetwork(neuralNetwork);
    }

    public SlimeInfo GetSlimeInfo()
    {
        return slimeInfo;
    }

    private void SetScale(float newScale)
    {
        scale = newScale;
        gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public float GetScale()
    {
        return scale;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public int GetGeneration()
    {
        return generation;
    }
    public float GetSaturation()
    {
        return saturation;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void CreateChild()
    {
        Debug.Log(name + " has eaten enough to have a child!"); 
        saturation = 50f * scale;
        numChildren += 1;
        SlimeManager.Instance().CreateSlime(gameObject);
    }
    
    public int GetNumChildren()
    {
        return numChildren;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private float scale = 1f;

    private float saturation = 30f;

    private int numChildren;

    private SlimeSpawner slimeSpawner;

    private GameObject closestFood;
    // Start is called before the first frame update
    void Start()
    {
        numChildren = 0;   
    }

    // Update is called once per frame
    void Update()
    {
        saturation -= 1 * scale * Time.deltaTime;

        if (closestFood == null) {
            GameObject [] foods = GameObject.FindGameObjectsWithTag("Food");
            if (foods.Length != 0) {
                closestFood = NearestFood(foods);
            }
        }

        if (closestFood != null) {
            Jump(closestFood);
        }

        if (saturation < 0)
        {
            Die();
        }
        else if (saturation > 100f * scale)
        {
            CreateChild();
        }
    }

    public void SetSlimeSpawner(SlimeSpawner slimeSpawner) {
        this.slimeSpawner = slimeSpawner;
    }

    void Jump(GameObject target)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        transform.LookAt(target.transform);
        rb.AddForce(transform.forward * Time.deltaTime, ForceMode.Impulse);
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
    

    public void SetScale(float newScale)
    {
        scale = newScale;
        gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
    }
    
    public float GetScale()
    {
        return scale;
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
        slimeSpawner.CreateChild(gameObject);
    }
    
    public int GetNumChildren()
    {
        return numChildren;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // Default saturation value of food
    private float _saturation = 25f;

    // public method to set _saturation
    public void SetSaturation(float newSaturation)
    {
        _saturation = newSaturation;
    }

    // Returns the saturation value associated with food
    public float GetSaturation()
    {
        return _saturation;
    }

    // Called if food is eaten, Deactivates the food object and returns it's saturation
    public float IsEaten()
    {
        FoodManager.Instance().DeactivateFood(gameObject);
        return _saturation;
    }
}

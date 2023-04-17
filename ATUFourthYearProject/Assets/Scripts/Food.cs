using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private float _saturation = 25f;

    public void SetSaturation(float newSaturation)
    {
        _saturation = newSaturation;
    }

    public float GetSaturation()
    {
        return _saturation;
    }

    public float IsEaten()
    {
        FoodManager.GetInstance().DeactivateFood(gameObject);
        return _saturation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private float saturation = 25f;

    public void SetSaturation(float newSaturation)
    {
        saturation = newSaturation;
    }

    public float GetSaturation()
    {
        return saturation;
    }

    public float IsEaten()
    {
        Destroy(gameObject);
        return saturation;
    }
}

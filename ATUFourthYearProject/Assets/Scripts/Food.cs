using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private float saturation = 25f;
    bool wasEaten = false;

    public void SetSaturation(float newSaturation)
    {
        saturation = newSaturation;
    }

    public float GetSaturation()
    {
        return saturation;
    }

    public bool WasEaten() {
        return wasEaten;
    }

    public float IsEaten()
    {
        wasEaten = true;
        Destroy(transform, 3.0f);
        return saturation;
    }
}

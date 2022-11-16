using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private float scale = 1f;

    private float food = 30f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (food < 0)
        {
            Die();
        }
        else if (food > 100f)
        {
            CreateChild();
        }

        food -= 1 * scale * Time.deltaTime;
    }

    private void OnMouseOver()
    {
        Debug.Log(transform.name);
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

    void Die()
    {
        Destroy(gameObject);
    }

    void CreateChild()
    {
        return;
    }
}

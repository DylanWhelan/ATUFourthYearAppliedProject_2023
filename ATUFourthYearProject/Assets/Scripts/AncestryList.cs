using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AncestryList : MonoBehaviour
{
    [SerializeField] private GameObject listItemPrefab;
    [SerializeField] private Transform contentTransform;
    [SerializeField] private ScrollRect scrollRect;

    private List<GameObject> listItems = new List<GameObject>();

    public void AddItem(string text)
    {
        GameObject newItem = Instantiate(listItemPrefab, contentTransform);
        newItem.GetComponent<Text>().text = text;
        listItems.Add(newItem);
        AdjustContentSize();
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < listItems.Count)
        {
            Destroy(listItems[index]);
            listItems.RemoveAt(index);
            AdjustContentSize();
        }
    }

    private void AdjustContentSize()
    {
        float itemHeight = listItemPrefab.GetComponent<RectTransform>().rect.height;
        float totalHeight = listItems.Count * itemHeight;
        contentTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, totalHeight);
        scrollRect.verticalNormalizedPosition = 1;
    }
}

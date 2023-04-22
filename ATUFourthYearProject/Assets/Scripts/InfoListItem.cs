using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _scale;
    [SerializeField] TextMeshProUGUI _speed;
    [SerializeField] TextMeshProUGUI _numChildren;
    [SerializeField] TextMeshProUGUI _generation;

    // Sets the text boxes for the info prefab
   public void init(SlimeInfo slimeInfo)
    {
        _name.SetText("Name: " + slimeInfo.SlimeName);
        _scale.SetText("Scale: {0:2}", slimeInfo.SlimeScale);
        _speed.SetText("Speed: {0:2}", slimeInfo.SlimeSpeed);
        _numChildren.SetText("Children: {0}", slimeInfo.SlimeChildren);
        _generation.SetText("Generation: {0}", slimeInfo.SlimeGeneration);
    }
}

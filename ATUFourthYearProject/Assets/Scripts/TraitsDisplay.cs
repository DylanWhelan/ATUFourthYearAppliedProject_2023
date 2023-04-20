using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;

public class TraitsDisplay : MonoBehaviour
{

    [SerializeField] private GameObject _slimeStatsDisplay;
    [SerializeField] private GameObject _slimeAncestryDisplay;
    [SerializeField] private GameObject _slimeAncestryPrefab;


    [SerializeField] private Transform _slimeAncenstryContent;

    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _scale;
    [SerializeField] private TextMeshProUGUI _speed;
    [SerializeField] private TextMeshProUGUI _numChildren;
    [SerializeField] private TextMeshProUGUI _generation;
    [SerializeField] private UnityEngine.UI.Slider _saturationSlider;

    Slime _slime;

    void Update() {
        if (_slime != null) {
            _saturationSlider.value = _slime.GetSaturation() / _slime.GetScale();
        }
    }

    public void UpdateStoredSlime() {
        _slime = null;
        _slimeStatsDisplay.SetActive(false);
        _slimeAncestryDisplay.SetActive(false);
    }

    public void UpdateStoredSlime(Slime slime) {
        _slime = slime;
        _slimeStatsDisplay.SetActive(true);
        _name.SetText("Name: " + _slime.name);
        _scale.SetText("Scale: {0:2}", _slime.GetScale());
        _speed.SetText("Speed: {0:2}", _slime.GetSpeed());
        _numChildren.SetText("Children: {0}", _slime.GetNumChildren());
        _generation.SetText("Generation: {0}", _slime.GetGeneration());

        if (_slime.GetSlimeInfo() != null)
        {
            foreach (Transform child in _slimeAncenstryContent)
            {
                Destroy(child.gameObject);
            }

            _slimeAncestryDisplay.SetActive(true);
            List <SlimeInfo> slimeInfoList = new List<SlimeInfo>();

            SlimeInfo slimeInfo = _slime.GetSlimeInfo();
            slimeInfoList.Add(slimeInfo);

            while (slimeInfo.ParentSlime != null)
            {
                slimeInfo = slimeInfo.ParentSlime;
                slimeInfoList.Add(slimeInfo);
            }

            foreach (SlimeInfo thisSlimeInfo in slimeInfoList)
            {
                GameObject item = Instantiate(_slimeAncestryPrefab, _slimeAncenstryContent);
                item.GetComponent<InfoListItem>().init(thisSlimeInfo);
            }
        }
    }
}

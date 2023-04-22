using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;

public class TraitsDisplay : MonoBehaviour
{
    // Ui area for slime stats display
    [SerializeField] private GameObject _slimeStatsDisplay;
    // Ui area for slime ancestry display
    [SerializeField] private GameObject _slimeAncestryDisplay;
    // Field where the prefab to display the information regarding parents can be set
    [SerializeField] private GameObject _slimeAncestryPrefab;

    // The content transform for the ancestry list, is needed so the list can be dynamically populated upon selecting a slime
    [SerializeField] private Transform _slimeAncenstryContent;

    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _scale;
    [SerializeField] private TextMeshProUGUI _speed;
    [SerializeField] private TextMeshProUGUI _numChildren;
    [SerializeField] private TextMeshProUGUI _generation;
    [SerializeField] private UnityEngine.UI.Slider _saturationSlider;

    Slime _slime;

    // If a slime is currently picked, update the saturation slider representing said slime each update
    void Update() {
        if (_slime != null) {
            _saturationSlider.value = _slime.GetSaturation() / _slime.GetScale();
        }
    }

    // UpdateStoredSlime method is polymorphic based on function overloading, if no other slime is specified, then the stored slime is set null and the fields are disabled
    public void UpdateStoredSlime() {
        _slime = null;
        _slimeStatsDisplay.SetActive(false);
        _slimeAncestryDisplay.SetActive(false);
    }

    // If there is a slime passed in
    public void UpdateStoredSlime(Slime slime) {

        // Slime is set to passed in slime, and dispalys are set to true
        _slime = slime;
        _slimeStatsDisplay.SetActive(true);
        // All text boxes are set to display statistics of the slime
        _name.SetText("Name: " + _slime.name);
        _scale.SetText("Scale: {0:2}", _slime.GetScale());
        _speed.SetText("Speed: {0:2}", _slime.GetSpeed());
        _numChildren.SetText("Children: {0}", _slime.GetNumChildren());
        _generation.SetText("Generation: {0}", _slime.GetGeneration());

        if (_slime.GetSlimeInfo() != null)
        {
            // Remove all list items in content for scrollable list
            foreach (Transform child in _slimeAncenstryContent)
            {
                Destroy(child.gameObject);
            }

            // Set the list display to be active
            _slimeAncestryDisplay.SetActive(true);

            // initialize list to store slimeInfos
            List <SlimeInfo> slimeInfoList = new List<SlimeInfo>();

            // get slimeInfo from _slime instance
            SlimeInfo slimeInfo = _slime.GetSlimeInfo();
            slimeInfoList.Add(slimeInfo);

            // While slimeInfo has parentSlime, iterate through, adding each slimeInfo to list
            while (slimeInfo.ParentSlime != null)
            {
                slimeInfo = slimeInfo.ParentSlime;
                slimeInfoList.Add(slimeInfo);
            }

            // Instantiate prefab for each list object to content transform so list can be scrolled, object pooling isn't used as number of list items is insignificant to affect performance
            foreach (SlimeInfo thisSlimeInfo in slimeInfoList)
            {
                GameObject item = Instantiate(_slimeAncestryPrefab, _slimeAncenstryContent);
                item.GetComponent<InfoListItem>().init(thisSlimeInfo);
            }
        }
    }
}

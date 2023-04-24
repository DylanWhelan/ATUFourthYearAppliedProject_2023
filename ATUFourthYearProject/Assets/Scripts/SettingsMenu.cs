using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenu;

    [SerializeField] private Slider _mutationChanceSlider;
    [SerializeField] private TextMeshProUGUI _mutationChanceText;

    [SerializeField] private Slider _mutationValueSlider;
    [SerializeField] private TextMeshProUGUI _mutationValueText;

    [SerializeField] private Slider _scaleChangeSlider;
    [SerializeField] private TextMeshProUGUI _scaleChangeText;

    [SerializeField] private Slider _scaleLowerBoundSlider;
    [SerializeField] private TextMeshProUGUI _scaleLowerBoundText;

    [SerializeField] private Slider _scaleUpperBoundSlider;
    [SerializeField] private TextMeshProUGUI _scaleUpperBoundText;

    [SerializeField] private Slider _speedChangeSlider;
    [SerializeField] private TextMeshProUGUI _speedChangeText;

    [SerializeField] private Slider _speedLowerBoundSlider;
    [SerializeField] private TextMeshProUGUI _speedLowerBoundText;

    [SerializeField] private Slider _speedUpperBoundSlider;
    [SerializeField] private TextMeshProUGUI _speedUpperBoundText;

    [SerializeField] private Slider _slimesToSpawnSlider;
    [SerializeField] private TextMeshProUGUI _slimesToSpawnText;

    [SerializeField] private Slider _foodPerIntervalSlider;
    [SerializeField] private TextMeshProUGUI _foodPerIntervalText;

    [SerializeField] private Slider _foodCapSlider;
    [SerializeField] private TextMeshProUGUI _foodCapText;

    public void OpenMenu()
    {
        _settingsMenu.SetActive(true);

        // Neural network settings
        _mutationChanceSlider.value = SimulationManager.Instance().MutationChance;
        _mutationChanceText.text = SimulationManager.Instance().MutationChance.ToString("D");
        _mutationValueSlider.value = SimulationManager.Instance().MutationValue;
        _mutationValueText.text = SimulationManager.Instance().MutationValue.ToString("0.00");

        // Scale related settings
        _scaleChangeSlider.value = SimulationManager.Instance().ScaleChange;
        _scaleChangeText.text = SimulationManager.Instance().ScaleChange.ToString("0.00");
        _scaleLowerBoundSlider.value = SimulationManager.Instance().ScaleLowerBound;
        _scaleLowerBoundText.text = SimulationManager.Instance().ScaleLowerBound.ToString("0.00");
        _scaleUpperBoundSlider.value = SimulationManager.Instance().ScaleUpperBound;
        _scaleUpperBoundText.text = SimulationManager.Instance().ScaleUpperBound.ToString("0.00");

        // Speed related settings
        _speedChangeSlider.value = SimulationManager.Instance().SpeedChange;
        _speedChangeText.text = SimulationManager.Instance().SpeedChange.ToString("0.00");
        _speedLowerBoundSlider.value = SimulationManager.Instance().SpeedLowerBound;
        _speedLowerBoundText.text = SimulationManager.Instance().SpeedLowerBound.ToString("0.00");
        _speedUpperBoundSlider.value = SimulationManager.Instance().SpeedUpperBound;
        _speedUpperBoundText.text = SimulationManager.Instance().SpeedUpperBound.ToString("0.00");

        // Other settings
        _slimesToSpawnSlider.value = SimulationManager.Instance().SlimesToSpawn;
        _slimesToSpawnText.text = SimulationManager.Instance().SlimesToSpawn.ToString("D");
        _foodPerIntervalSlider.value = SimulationManager.Instance().FoodPerInterval;
        _foodPerIntervalText.text = SimulationManager.Instance().FoodPerInterval.ToString("D");
        _foodCapSlider.value = SimulationManager.Instance().FoodCap;
        _foodCapText.text = SimulationManager.Instance().FoodCap.ToString("D");
    }

    public void CloseMenu()
    {
        _settingsMenu.SetActive(false);
    }

    public void ResetMenu()
    {
        PlayerPrefs.DeleteAll();
        SimulationManager.Instance().LoadPrefs();
        CloseMenu();
    }

    public void MutationChanceSlider(float valueToCast)
    {
        int value = (int) valueToCast;
        SimulationManager.Instance().MutationChance = value;
        _mutationChanceText.text = SimulationManager.Instance().MutationChance.ToString("D");
    }

    public void MutationValueSlider(float value)
    {
        SimulationManager.Instance().MutationValue = value;
        _mutationValueText.text = value.ToString("0.00");
    }

    public void ScaleChangeSlider(float value)
    {
        SimulationManager.Instance().ScaleChange = value;
        _scaleChangeText.text = value.ToString("0.00");
    }

    public void ScaleLowerBoundSlider(float value)
    {
        SimulationManager.Instance().ScaleLowerBound = value;
        _scaleLowerBoundText.text = value.ToString("0.00");
    }

    public void ScaleUpperBoundSlider(float value)
    {
        SimulationManager.Instance().ScaleUpperBound = value;
        _scaleUpperBoundText.text = value.ToString("0.00");
    }

    public void SpeedChangeSlider(float value)
    {
        SimulationManager.Instance().SpeedChange = value;
        _speedChangeText.text = value.ToString("0.00");
    }

    public void SpeedLowerBoundSlider(float value)
    {
        SimulationManager.Instance().SpeedLowerBound = value;
        _speedLowerBoundText.text = value.ToString("0.00");
    }

    public void SpeedUpperBoundSlider(float value)
    {
        SimulationManager.Instance().SpeedUpperBound = value;
        _speedUpperBoundText.text = value.ToString("0.00");
    }

    public void SlimesToSpawnSlider(float valueToCast)
    {
        int value = (int) valueToCast;
        SimulationManager.Instance().SlimesToSpawn = value;
        _slimesToSpawnText.text = value.ToString("D");
    }

    public void FoodPerIntervalSlider(float valueToCast)
    {
        int value = (int) valueToCast;
        SimulationManager.Instance().FoodPerInterval = value;
        _foodPerIntervalText.text = value.ToString("D");
    }

    public void FoodCapSlider(float valueToCast)
    {
        int value = (int)valueToCast;
        SimulationManager.Instance().FoodCap = value;
        _foodCapText.text = value.ToString("D");
    }
}

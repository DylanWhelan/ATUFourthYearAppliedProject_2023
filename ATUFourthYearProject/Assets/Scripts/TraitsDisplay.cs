using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TraitsDisplay : MonoBehaviour
{
    public TextMeshProUGUI _traitOutput;

    Slime _slime;

    void Update() {
        if (_slime != null) {
            string text = "Slime: " + _slime.name + "<br>Size: {0:2}<br>Saturation: {1:2}";
            _traitOutput.SetText(text, _slime.GetScale(), _slime.GetSaturation());
        }
        else
        {
            _traitOutput.text = "No slime selected";
        }
    }

    public void UpdateStoredSlime() {
        this._slime = null;
    }

    public void UpdateStoredSlime(Slime slime) {
        this._slime = slime;
    }
}

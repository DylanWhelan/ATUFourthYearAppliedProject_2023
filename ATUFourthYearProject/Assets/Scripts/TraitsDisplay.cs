using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TraitsDisplay : MonoBehaviour
{
    public TextMeshProUGUI traitOutput;

    Slime slime;

    void Update() {
        if (slime != null) {
            string text = "Slime: " + slime.name + "<br>Size: {0:2}<br>Saturation: {1:2}";
            traitOutput.SetText(text, slime.GetScale(), slime.GetSaturation());
        }
        else
        {
            traitOutput.text = "No slime selected";
        }
    }

    public void UpdateStoredSlime() {
        this.slime = null;
    }

    public void UpdateStoredSlime(Slime slime) {
        this.slime = slime;
    }
}

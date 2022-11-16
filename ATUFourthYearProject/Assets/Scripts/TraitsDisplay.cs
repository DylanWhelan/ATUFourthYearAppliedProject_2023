using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TraitsDisplay : MonoBehaviour
{
    public TextMeshProUGUI traitOutput;
    // Start is called before the first frame update
    void Start()
    {
        TextUpdate();
    }

    public void TextUpdate()
    {
        Debug.Log("Was here!");
        traitOutput.text = "No slime selected";
    }

    public void TextUpdate(Slime slime)
    {
        string text = "Slime: " + slime.name + "<br>Size: {0:2}";
        traitOutput.SetText(text, slime.GetScale());
    }
}

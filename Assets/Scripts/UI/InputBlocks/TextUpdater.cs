using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponent<TextMeshProUGUI>();
    }

    public void setText(float f)
    {
        text.text = f.ToString();
    }
}

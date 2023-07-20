using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarkerNode : Node
{
    ColorDisplay[] colorDisplays;

    int colorIndex = 0;
    Color[] colors = { Color.red, Color.green, Color.blue }; 
    string[] colorNames = { "Red", "Green", "Blue" };

    internal override void exStart()
    {
        base.exStart();
        colorDisplays = this.GetComponentsInChildren<ColorDisplay>();
    }

    internal override void exUpdate()
    {
        base.exUpdate();
        if (isDragged && Input.GetKeyDown(KeyCode.Q)) prevColor();
        if (isDragged && Input.GetKeyDown(KeyCode.E)) nextColor();
    }

    public override void setDrag(bool dragged)
    {
        base.setDrag(dragged);
        foreach (ColorDisplay colorDisplay in colorDisplays) colorDisplay.activeBg.SetActive(dragged);
    }

    public string getColorName()
    {
        return colorNames[colorIndex];
    }

    private void prevColor() 
    {
        if (colorIndex > 0) colorIndex--;
        else colorIndex = colors.Length - 1;
        foreach (ColorDisplay colorDisplay in colorDisplays) colorDisplay.image.color = colors[colorIndex];
    }

    private void nextColor()
    {
        if (colorIndex < colors.Length - 1) colorIndex++;
        else colorIndex = 0;
        foreach (ColorDisplay colorDisplay in colorDisplays) colorDisplay.image.color = colors[colorIndex];
    }
}

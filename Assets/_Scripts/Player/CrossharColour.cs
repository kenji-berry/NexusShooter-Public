using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairColorSelector : MonoBehaviour
{
    public TMP_Dropdown colorDropdown;
    public GameController gameController;

    void Start()
    {
        colorDropdown.onValueChanged.AddListener(delegate { OnColorChanged(); });
        colorDropdown.value = 0; // Set default to white
        OnColorChanged();
    }

    void OnColorChanged()
    {
        Color selectedColor = Color.white; // Default 

        switch (colorDropdown.value)
        {
            case 0:
                selectedColor = Color.white;
                break;
            case 1:
                selectedColor = Color.red;
                break;
            case 2:
                selectedColor = Color.green;
                break;
            case 3:
                selectedColor = Color.blue;
                break;
            case 4:
                selectedColor = Color.yellow;
                break;
            case 5:
                selectedColor = Color.cyan;
                break;
            case 6:
                selectedColor = Color.magenta; // Purple
                break;
        }

        gameController.crosshairVerticalTopLine.color = selectedColor;
        gameController.crosshairVerticalBottomLine.color = selectedColor;
        gameController.crosshairHorizontalLeftLine.color = selectedColor;
        gameController.crosshairHorizontalRightLine.color = selectedColor;
    }
}

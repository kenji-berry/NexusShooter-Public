using UnityEngine;
using UnityEngine.UI;

public class CrosshairCustomizationController : MonoBehaviour
{
    public Slider widthSlider;
    public Slider lengthSlider;
    public Slider spacingSlider;
    public GameController gameController;

    void Start()
    {
        widthSlider.onValueChanged.AddListener(delegate { OnWidthChanged(); });
        lengthSlider.onValueChanged.AddListener(delegate { OnLengthChanged(); });
        spacingSlider.onValueChanged.AddListener(delegate { OnSpacingChanged(); });

        // Set default values
        widthSlider.value = gameController.crosshairVerticalTopLine.rectTransform.sizeDelta.x;
        lengthSlider.value = gameController.crosshairVerticalTopLine.rectTransform.sizeDelta.y;
        spacingSlider.value = gameController.crosshairVerticalTopLine.rectTransform.anchoredPosition.y;
    }

    void OnWidthChanged()
    {
        float newWidth = widthSlider.value;
        gameController.crosshairVerticalTopLine.rectTransform.sizeDelta = new Vector2(newWidth, gameController.crosshairVerticalTopLine.rectTransform.sizeDelta.y);
        gameController.crosshairVerticalBottomLine.rectTransform.sizeDelta = new Vector2(newWidth, gameController.crosshairVerticalBottomLine.rectTransform.sizeDelta.y);
        gameController.crosshairHorizontalLeftLine.rectTransform.sizeDelta = new Vector2(gameController.crosshairHorizontalLeftLine.rectTransform.sizeDelta.x, newWidth);
        gameController.crosshairHorizontalRightLine.rectTransform.sizeDelta = new Vector2(gameController.crosshairHorizontalRightLine.rectTransform.sizeDelta.x, newWidth);
    }

    void OnLengthChanged()
    {
        float newLength = lengthSlider.value;
        gameController.crosshairVerticalTopLine.rectTransform.sizeDelta = new Vector2(gameController.crosshairVerticalTopLine.rectTransform.sizeDelta.x, newLength);
        gameController.crosshairVerticalBottomLine.rectTransform.sizeDelta = new Vector2(gameController.crosshairVerticalBottomLine.rectTransform.sizeDelta.x, newLength);
        gameController.crosshairHorizontalLeftLine.rectTransform.sizeDelta = new Vector2(newLength, gameController.crosshairHorizontalLeftLine.rectTransform.sizeDelta.y);
        gameController.crosshairHorizontalRightLine.rectTransform.sizeDelta = new Vector2(newLength, gameController.crosshairHorizontalRightLine.rectTransform.sizeDelta.y);
    }

    void OnSpacingChanged()
    {
        float newSpacing = spacingSlider.value;
        gameController.crosshairVerticalTopLine.rectTransform.anchoredPosition = new Vector2(gameController.crosshairVerticalTopLine.rectTransform.anchoredPosition.x, newSpacing);
        gameController.crosshairVerticalBottomLine.rectTransform.anchoredPosition = new Vector2(gameController.crosshairVerticalBottomLine.rectTransform.anchoredPosition.x, -newSpacing);
        gameController.crosshairHorizontalLeftLine.rectTransform.anchoredPosition = new Vector2(-newSpacing, gameController.crosshairHorizontalLeftLine.rectTransform.anchoredPosition.y);
        gameController.crosshairHorizontalRightLine.rectTransform.anchoredPosition = new Vector2(newSpacing, gameController.crosshairHorizontalRightLine.rectTransform.anchoredPosition.y);
    }
}
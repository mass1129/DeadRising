using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealthBar : MonoBehaviour
{
    [Header("Health")]
    public Image healthForeground;
    public Image healthBackground;
    public TMP_Text  healthText;


    public void SetHealthBarPercentage(float percentage)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        healthForeground.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        healthText.text = (percentage*100).ToString();
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEXPBar : MonoBehaviour
{
    public TMP_Text killNumText;
    public TMP_Text levelNumText;

    public Image expForeground;
    public Image expBackground;

    public void SetEXPBarPercentage(float percentage)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        expForeground.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        killNumText.text = PlayerEXP.instance.KillNum.ToString();
        levelNumText.text = PlayerEXP.instance.Level.ToString();
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

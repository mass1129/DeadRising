using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Weapon")]
    public TMP_Text[] ammoCounttext;
    public TMP_Text[] clipCountText;
    public Image[] outLineImg;
    public GameObject arrow2Slot;

    [Header("Health")]
    public GameObject healthUI;
    float parentWidth;
    public Image healthForeground;
    public Image healthBackground;
    public TMP_Text healthText;


    private void Start()
    {
        parentWidth = healthUI.GetComponent<RectTransform>().rect.width;
        DeactiveSlotUI();
        arrow2Slot.SetActive(false);
    }

    public void Refresh(int ammoCount, int clipCount, int activeWeaponIndex, bool uninfinitybullet)
    {
        if (!uninfinitybullet) return;
        if (activeWeaponIndex < 0 || activeWeaponIndex > 1) return;
        ammoCounttext[activeWeaponIndex].text = ammoCount.ToString();
        clipCountText[activeWeaponIndex].text = clipCount.ToString();

    }

    public void ActiveSlotUI(int index)
    {

        if (index < 0 || index > 1) return;
        for (int i = 0; i < outLineImg.Length; i++)
        {
            if (i == index)
                outLineImg[i].enabled = true;
            else
                outLineImg[i].enabled = false;
        }

    }
    
    public void DeactiveSlotUI()
    {
        for (int i = 0; i < outLineImg.Length; i++)
        {
                outLineImg[i].enabled = false;
        }

    }
    public void SetHealthBarPercentage(float percentage)
    {
        float width = parentWidth * percentage;
        healthForeground.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        healthText.text = (percentage * 100).ToString();
    }
}
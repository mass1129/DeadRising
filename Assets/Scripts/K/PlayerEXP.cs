using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerEXP : MonoBehaviour
{
    public static PlayerEXP instance;

    int _killNum = 0;
    int _levelNum = 1;
    int _killPerLevel = 0;
    public int multipleLevel = 2;

    public bool upgradeWeapon;

    public GameObject expUI;
    float parentWidth;
    public TMP_Text killNumText;
    public TMP_Text levelNumText;

    public Image expForeground;
    public Image expBackground;


    private void Awake()
    {
        parentWidth = expUI.GetComponent<RectTransform>().rect.width;
        SetEXPBarPercentage(KillPerLevel / (Level * multipleLevel));
        KillPerLevel = 0;
        upgradeWeapon = false;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            gameObject.SetActive(false);
        }
        if (SceneManager.GetActiveScene().buildIndex != 7)
        {

            gameObject.SetActive(true);
        }
    }


    public int KillNum
    {
        get
        {
            return _killNum;
        }
        set
        {
            _killNum = value;
            KillPerLevel++;
            
        }
    }

    public  int KillPerLevel
    {
        get
        {
            return _killPerLevel;
        }
        set
        {
            _killPerLevel = value;
            if(_killPerLevel>=(Level* multipleLevel))
            {
                Level++;
                _killPerLevel = 0;
                upgradeWeapon = true;

            }
            float percent = (float)KillPerLevel / ((float)Level * multipleLevel);
            
            SetEXPBarPercentage(percent);
           
        }
    }
    public int Level
    {
        get
        {
            return _levelNum;
        }
        set 
        {
            _levelNum = value;
        }
    }

   
   

    public void SetEXPBarPercentage(float percentage)
    {
        float width = parentWidth * percentage;
        expForeground.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        killNumText.text = KillNum.ToString();
        levelNumText.text = Level.ToString();

    }
}

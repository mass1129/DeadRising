using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;


public class PlayerEXP : MonoBehaviour
{
    public static PlayerEXP instance;

    UIEXPBar expBar;

    int _killNum = 0;
    int _levelNum = 1;
    int _killPerLevel = 0;


    private void Awake()
    {
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
            if(_killPerLevel>=(Level*5))
            {
                Level++;
                _killPerLevel = 0;
                upgradeWeapon = true;

            }
            float percent = (float)KillPerLevel / ((float)Level * 5);
            
            expBar.SetEXPBarPercentage(percent);
           
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

    public bool upgradeWeapon;
   


    void Start()
    {
        expBar = GetComponentInChildren<UIEXPBar>();
        expBar.SetEXPBarPercentage(KillPerLevel / (Level * 5));
        KillPerLevel = 0;
        upgradeWeapon = false;

    }

    // Update is called once per frame
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
}

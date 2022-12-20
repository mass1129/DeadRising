using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_UI_Explain : MonoBehaviour
{
    public GameObject escapeUI;
    public GameObject settingUI;
    // Start is called before the first frame update
    void Start()
    {
        escapeUI.SetActive(false);
        settingUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void BtnBack()
    {
        escapeUI.SetActive(false);
        settingUI.SetActive(false);
    }
}

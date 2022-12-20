using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_UI_Start_btn : MonoBehaviour
{
    public GameObject startUI;
    public GameObject startBtnUI;
    public GameObject explainUI;
    public GameObject settingUI;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void btnStart()
    {
        startBtnUI.SetActive(false);
        startUI.SetActive(false);
        explainUI.SetActive(false);
        settingUI.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void btnExplain()
    {
        explainUI.SetActive(true);
    }

    public void btnSetting()
    {
        settingUI.SetActive(true);
    }

    public void btnExit()
    {
        Application.Quit();
    }
}

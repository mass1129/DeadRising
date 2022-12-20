using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JH_UI_Manager : MonoBehaviour
{
    public GameObject escapeUI;
    public GameObject settingUI;
    public GameObject explainUI;

    public GameObject startUI;
    public GameObject startbtnUI;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            startUI.SetActive(false);
            startbtnUI.SetActive(false);
            
        }

        settingUI.SetActive(false);
        escapeUI.SetActive(false);
        explainUI.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (startUI.activeInHierarchy || escapeUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void test()
    {
        Debug.Log("테스트");
    }
}

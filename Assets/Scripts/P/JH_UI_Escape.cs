using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JH_UI_Escape : MonoBehaviour
{
    public GameObject escapeUI;
    public GameObject startUI;
    public GameObject startBtnUI;

    public GameObject explainUI;
    public GameObject settingUI;
    bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        escapeUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!explainUI.activeInHierarchy && !settingUI.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !isActive && !startUI.activeInHierarchy && !startBtnUI.activeInHierarchy)
            {
                isActive = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                escapeUI.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isActive && !startUI.activeInHierarchy && !startBtnUI.activeInHierarchy)
            {
                isActive = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
                escapeUI.SetActive(false);
            }
        }
    }

    public void InitialScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("1_opening");
    }

    public void CheckPoint()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

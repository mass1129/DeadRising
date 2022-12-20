using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JH_SceneChanger : MonoBehaviour
{
    public int toScene;
    public GameObject loadingScreen;
    public Slider slider;

    bool oneTrigger = false;
    private void OnTriggerEnter(Collider other)

    {
        if (oneTrigger) return;

        if (other.CompareTag("Player") || other.CompareTag("Car"))
        {
            
            LoadLevel(toScene);
            oneTrigger = true;

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)&&toScene != 1)
        {
            BackSceneLoad();
        }
        if (Input.GetKeyDown(KeyCode.P)&&toScene!=8)
        {
            NextSceneLoad();
        }

        if(SceneManager.GetActiveScene().buildIndex == 7)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
    }

    public void NextSceneLoad()
    {
        
        LoadLevel(toScene);
        
        //SceneManager.LoadScene(toScene);
    }
    public void BackSceneLoad()
    {
        
        LoadLevel(toScene - 2);
        
           
        //SceneManager.LoadScene(toScene - 2);
    }


    public void LoadLevel(int sceneIndex)
    {
        
        StartCoroutine(LoadAsynchronously(sceneIndex));
        

    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;
            yield return null;
        }

       
            
        



    }
}
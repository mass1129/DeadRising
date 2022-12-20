using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OpeningCinemachine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StartCoroutine(OpeningScene());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator OpeningScene()
    {
        yield return new WaitForSeconds(18.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

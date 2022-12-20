using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgSound;
    public AudioClip[] bgList;
    public float bgVolume= 0.1f;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        

        for (int i = 0; i < bgList.Length; i++)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 7) return;

            if (arg0.name == bgList[i].name)
            BGSoundPlay(bgList[i]);
            else
                BGSoundPlay(bgList[0]);
        }
    }



    public void BGSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();
    }


}

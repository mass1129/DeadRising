using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JH_SceneGoalTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] goalUi;
    [SerializeField] GameObject[] semiUi;
    [SerializeField] GameObject[] preUi;

    Animator anim;
    private void Start() {
        if(preUi[1]){
            anim = preUi[1].GetComponent<Animator>();
        }else if(semiUi[1]){
            anim = semiUi[1].GetComponent<Animator>();
        }

        if(preUi[0]){
            preUi[0].SetActive(true);
            preUi[1].SetActive(true);

            if(preUi[0].activeInHierarchy){
                semiUi[0].SetActive(false);
                semiUi[1].SetActive(false);
            }else{
                 semiUi[0].SetActive(true);
                 semiUi[1].SetActive(true);
            }
        }

        goalUi[0].SetActive(false);
        goalUi[1].SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){


        goalUi[0].SetActive(true);
        goalUi[1].SetActive(true);

        semiUi[0].SetActive(false);
        anim.SetTrigger("clear");
        if(semiUi[1]){
            anim = semiUi[1].GetComponent<Animator>();
            anim.SetTrigger("clear");
        }
        StartCoroutine(screenMissionfalse());
        }
    }

    IEnumerator screenMissionfalse(){
        yield return new WaitForSeconds(1);
        semiUi[1].SetActive(false);
    }
}

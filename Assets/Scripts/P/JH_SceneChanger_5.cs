using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_SceneChanger_5 : MonoBehaviour
{   
    [SerializeField] GameObject uiFindaidKit;
    [SerializeField] GameObject uiOpenDoor;
    [SerializeField] GameObject uiFindVaccine;

    [SerializeField] GameObject aidKit;
    [SerializeField] GameObject door;
    [SerializeField] GameObject vaccine;
    [SerializeField] GameObject wayoutDoor;

    [SerializeField] GameObject uiaidKitDir;
    [SerializeField] GameObject uiDoorDir;
    [SerializeField] GameObject uiVaccineDir;
   

    JH_DoorActive doorActive;

    Animator anim;
    
    void Start()
    {
        uiFindaidKit.SetActive(true);
        uiOpenDoor.SetActive(false);
        uiFindVaccine.SetActive(false);

        uiaidKitDir.SetActive(true);
        uiDoorDir.SetActive(false);
        uiVaccineDir.SetActive(false);
        

        doorActive = door.GetComponent<JH_DoorActive>();
    }


    void Update()
    {
        if(aidKit.activeInHierarchy == false && uiFindaidKit.activeInHierarchy == true){
            uiOpenDoor.SetActive(true);
            uiDoorDir.SetActive(true);
            uiaidKitDir.SetActive(false);

            anim = uiFindaidKit.GetComponent<Animator>();
            anim.SetTrigger("clear");
            StartCoroutine(uiClear());
        }

        if(doorActive.isOpen && uiOpenDoor.activeInHierarchy == true){
            uiFindVaccine.SetActive(true);
            uiVaccineDir.SetActive(true);
            uiDoorDir.SetActive(false);

            anim = uiOpenDoor.GetComponent<Animator>();
            anim.SetTrigger("clear");
            StartCoroutine(uiClear());
        }
        if(vaccine.activeInHierarchy == false && uiFindVaccine.activeInHierarchy == true){
            

            doorActive = wayoutDoor.GetComponent<JH_DoorActive>();
            doorActive.isOpen = true;
        }

    }

    IEnumerator uiClear(){
        yield return new WaitForSeconds(1);
        uiFindaidKit.SetActive(false);
    }
}

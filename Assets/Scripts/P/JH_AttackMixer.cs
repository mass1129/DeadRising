using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JH_AttackMixer : MonoBehaviour
{

    JH_isHeadHit head;
    JH_BarricadeActive barricade;

    Scene currentScene;
    string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 8){
            head = other.GetComponentInChildren<JH_isHeadHit>();

            if(head){
                if(sceneName == "4_Highway"){
                    head.OnRaycastHeadHit(new Vector3(0, 1, -3));
                }else if(sceneName == "6_WayOut"){
                    head.OnRaycastHeadHit(new Vector3(-3, 1, 0));
                }
            }
        }else if(other.gameObject.layer == 6){
            barricade = other.GetComponent<JH_BarricadeActive>();
            barricade.OnRaycastHeadHit(new Vector3(-3, 1, 0));
        }
    }
}

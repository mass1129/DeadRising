using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_CarUI : MonoBehaviour
{

    [SerializeField] Text uiText;
    [SerializeField] string PlayerName;
    private GameObject target;
    float alpha;
    

    // Start is called before the first frame update
    void Start()
    {
        uiText.color = new Color(1, 1, 1, alpha);
        target = GameObject.Find(PlayerName);
    }

    // Update is called once per frame
    void Update()
    {
        if(target.activeInHierarchy){

        if(Vector3.Distance(target.transform.position, transform.position) <= 20.0f){
            uiText.color = new Color(1, 1, 1, alpha);
            if(alpha <= 1) {
                alpha += Time.deltaTime;
            }
        }
        
        if(Vector3.Distance(target.transform.position, transform.position) >= 20.0f){
            uiText.color = new Color(1, 1, 1, alpha);
            if(alpha >= 0) {
                alpha -= Time.deltaTime;
            }
        }
        }else{
            uiText.color = new Color(1, 1, 1, 0);
        }
    }
}

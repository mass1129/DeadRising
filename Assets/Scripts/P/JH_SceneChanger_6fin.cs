using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_SceneChanger_6fin : MonoBehaviour
{   
    [SerializeField] GameObject uiFin;

    // Start is called before the first frame update
    void Start()
    {
        uiFin.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Car")){
            uiFin.SetActive(true);
        }
    }
}

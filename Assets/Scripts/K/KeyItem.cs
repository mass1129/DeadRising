using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        detectArea = transform.GetChild(0).gameObject;
        target = GameObject.FindGameObjectWithTag("MainCamera");
    }
    GameObject detectArea;



    // Update is called once per frame
    GameObject target;
    // Start is called before the first frame update
   

    // Update is called once per framesssss
    void Update()
    {
        detectArea.transform.LookAt(target.transform.position);
    }



}

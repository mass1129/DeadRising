using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_LookAt : MonoBehaviour
{
    GameObject target;
    GameObject isMe;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per framesssss
    void Update()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        distance = distance/1000;
        transform.localScale = new Vector3(distance,distance,distance);
        transform.LookAt(target.transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_DoorActive : MonoBehaviour
{
    public Animator anim;
    public bool isOpen;
    public Collider col;
    public GameObject ui;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();
        anim.SetBool("isClose",true);
        col.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen){    
            anim.SetBool("isClose",false);
            col.enabled = false;
        }else{    
            anim.SetBool("isClose",true);
            col.enabled = true;
        }
    }
}

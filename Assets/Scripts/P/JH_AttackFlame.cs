using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_AttackFlame : MonoBehaviour
{
    [SerializeField] BoxCollider flameCollider;
    [SerializeField] ParticleSystem flame;
    [SerializeField] ParticleSystem dust;

    ParticleSystem.MainModule main;
    ParticleSystem.MainModule sub;
    
    int value;

    bool bigMode = false;
 

    // Start is called before the first frame update
    void Start()
    {   
        main = flame.main;
        sub = dust.main;
        flameCollider.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        main.maxParticles = value;
        sub.maxParticles = value;

        if(gameObject.transform.Find("Player")){
            if(Input.GetButton("Fire1")){
            if(value <= 1000){
                value += 15;
            }
            flameCollider.enabled = true;
        }

        if(Input.GetButtonUp("Fire1")){

            flameCollider.enabled = false;
        }


        if(Input.GetButtonDown("Fire2")){
            swapMode();
        }

        if(value >= 0){
            value -= 10;
        }
        }


    }
    
    private void swapMode(){
        var shape = flame.shape;
            bigMode = !bigMode;
            if(bigMode){
                shape.angle = 25;
                main.startSize = 5;
                flameCollider.size = new Vector3(9,7,15);
            }else{
                shape.angle = 5;
                main.startSize = 1;
                flameCollider.size = new Vector3(4,3,12);
            }
        main = flame.main;
    }

}

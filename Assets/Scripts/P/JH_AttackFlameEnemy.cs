using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_AttackFlameEnemy : MonoBehaviour
{
    [SerializeField] 
    GameObject[] burningFxPreArray = new GameObject[3];
    GameObject burningFxPre;
    GameObject burningFx;

    int random;

    float timer = 5f;
    bool burningBlockTime = false;

    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(0,2);
        burningFxPre = burningFxPreArray[random];  
    }

    // Update is called once per frame
    void Update()
    {
        if(burningBlockTime){
            timer -= Time.deltaTime;
            if(timer <= 0){
                burningBlockTime = false;
                timer = 5;    
                }
        }
    }

    private void OnTriggerEnter(Collider other) {
            if(other.name.Contains("FlameVFX") && !burningBlockTime){
                burningFx = Instantiate(burningFxPre, transform.position + new Vector3(0, 0.7f, 0), transform.rotation);
                burningFx.transform.localScale = new Vector3(0.008f,0.008f,0.008f);
                burningFx.transform.parent = transform;
                burningBlockTime = true;
            }    
                 

    }
}

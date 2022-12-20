using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JH_DistanceUI : MonoBehaviour
{
    public Text distanceText;
    float distnce;

    // Start is called before the first frame update
    void Start()
    {
        distanceText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        if(target){

        distnce = (Vector3.Distance(transform.position, target.transform.position))-1.44f;

        if(distnce <= 0){
            distnce = 0;
        }
        distanceText.text = $"{distnce.ToString("F2")}M";

        }
        
    }
}

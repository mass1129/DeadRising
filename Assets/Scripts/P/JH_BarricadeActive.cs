using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_BarricadeActive : MonoBehaviour
{
    public BoxCollider col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        public void OnRaycastHeadHit(Vector3 force) {

        Rigidbody rigi = GetComponent<Rigidbody>();
        
        rigi.AddExplosionForce(8000, transform.position, 1000 );
        //rigi.AddForce(force * 5, ForceMode.VelocityChange);

        //col.enabled = false;
    }

}

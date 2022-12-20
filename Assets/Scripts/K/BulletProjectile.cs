using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    private Rigidbody bulletRigidbody;
    [SerializeField] private Transform hitEffect;
    [SerializeField] private Transform missEffect;

    [SerializeField] float speed=10f;
    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {   
        if(other.GetComponent<BulletTarget>() != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(missEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}

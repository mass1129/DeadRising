using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_isHeadHit : MonoBehaviour
{
    public GameObject parent;
    private Animator _animator;

    public GameObject head;
    public GameObject replaceHead;

    GameObject findHead;

    public JH_EnemyHealth health;
    public SphereCollider col;

    private void Start() {
        _animator = parent.GetComponent<Animator>();
        health = parent.GetComponent<JH_EnemyHealth>();
        col = GetComponent<SphereCollider>();
    }

    public void OnRaycastHeadHit(Vector3 force) {
        health.TakeDamage(30, force);

        if (health.currentHealth <= 0){
            _animator.enabled = false;
            Destroy(head);
            findHead = Instantiate(replaceHead, transform.position, transform.rotation);
            var rigidBodies = findHead.GetComponentsInChildren<Rigidbody>();
            
            foreach (var rigidBody in rigidBodies){
                rigidBody.AddExplosionForce(800, findHead.transform.position, 1000 );
                rigidBody.AddForce(force * 18, ForceMode.VelocityChange);
            }
            col.enabled = false;
        }

        
    }


}

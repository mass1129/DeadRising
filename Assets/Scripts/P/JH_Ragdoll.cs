using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_Ragdoll : MonoBehaviour
{
    Rigidbody[] rigiBodies;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigiBodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();

        DeactivateRagdoll();
    }

    public void DeactivateRagdoll(){
        foreach(var rigidbody in rigiBodies){
            rigidbody.isKinematic = true;
        }
        animator.enabled = true;
    }

    public void ActivateRagdoll(){
        foreach(var rigidbody in rigiBodies){
            rigidbody.isKinematic = false;
        }
        animator.enabled = false;
    }

        public void ApplyForce(Vector3 force){
        var rigidBody = animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        
        rigidBody.AddForce(force, ForceMode.VelocityChange);
    }

}

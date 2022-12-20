using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    Animator animator;
    Vector2 input;
    CharacterController cc;
    Vector3 rootMotion;


    void Start()
    {
        animator = GetComponent<Animator>();
        cc= GetComponent<CharacterController>();    
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);

    }
    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void FixedUpdate()
    {
        cc.Move(rootMotion);
        rootMotion = Vector3.zero;
    }

}

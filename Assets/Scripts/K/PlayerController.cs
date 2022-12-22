
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour
{

    public Animator rigController;
    public float jumpHeight;
    public float gravity = -15.0f;
    public float stepDown;
    public float airControl;
    public float jumpDamp;
    public float groundSpeed;
    public float pushPower = 2.0F;
    
    Animator animator;
    private CharacterController cc;
    ActiveWeapon activeWeapon;
    CharacterAiming characterAiming;
    [System.NonSerialized]public Vector2 inputVector;

    Vector3 rootMotion;
    Vector3 velocity;
    bool isJumping = false;
    int isSprintingParam = Animator.StringToHash("isSprinting");


    [SerializeField] AudioClip[] audioClip;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        

    }
   
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        activeWeapon = GetComponent<ActiveWeapon>();
        characterAiming = GetComponent<CharacterAiming>();
    }


    void Update()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
    }


    private void FixedUpdate()
    {   
        if(isJumping)
        {
            UpdateInAir();
        }
        else
        {
            UpdateOnGround();
        }

    }


    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3f)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = pushDir * pushPower;
    }


    private void UpdateOnGround()
    {
        animator.SetFloat("InputX", inputVector.x);
        animator.SetFloat("InputY", inputVector.y);

        UpdateIsSprinting();

        Vector3 stepForwardAmount = rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;

        cc.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (!cc.isGrounded)
        {
            SetinAir(0);
        }
    }

    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        displacement += CalculateAirControl();
        cc.Move(velocity * Time.fixedDeltaTime);
        isJumping = !cc.isGrounded;
        rootMotion = Vector3.zero;
        animator.SetBool("isJumping", isJumping);
    }

    void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetinAir(jumpVelocity);
        }
    }

    private void SetinAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity * jumpDamp * groundSpeed;
        velocity.y = jumpVelocity;
        animator.SetBool("isJumping", true);
    }

    Vector3 CalculateAirControl()
    {
        return ((transform.forward * inputVector.y) + (transform.right * inputVector.x)) * (airControl / 100);
    }



    bool IsSprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = activeWeapon.IsFiring();
        bool isReloading = activeWeapon.isReloading;
        bool isChangingWeapon = activeWeapon.isChangingWeapon;
        bool isAiming = characterAiming.isAiming;
        return isSprinting && !isFiring && !isReloading && !isChangingWeapon && !isAiming;

    }

    private void UpdateIsSprinting()
    {
        bool isSprinting = IsSprinting();
        animator.SetBool(isSprintingParam, isSprinting);
        rigController.SetBool(isSprintingParam, isSprinting);
    }

    
    

    
   
    private void insideStep()
    {
        AudioClip clip = GetRandomClip(0, 4);
        audioSource.PlayOneShot(clip);

    }

    private AudioClip GetRandomClip(int a, int b)
    {
        int index = Random.Range(a, b);
        return audioClip[index];
    }

}
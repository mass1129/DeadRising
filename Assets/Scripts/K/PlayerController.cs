
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
    
    private void insideStep()
    {
        AudioClip clip = GetRandomClip(0,4);
        audioSource.PlayOneShot(clip);

    }

    private AudioClip GetRandomClip(int a, int b)
    {
        int index = Random.Range(a, b);
        return audioClip[index];
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

        animator.SetFloat("InputX", inputVector.x);
        animator.SetFloat("InputY", inputVector.y);

        UpdateIsSprinting();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    bool IsSprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = activeWeapon.IsFiring();
        bool isReloading = activeWeapon.isReloading;
        bool isChangingWeapon = activeWeapon.isChangingWeapon;
        bool isAiming = characterAiming.isAiming;
        return isSprinting && !isFiring&& !isReloading && !isChangingWeapon&&!isAiming;

    }

    private void UpdateIsSprinting()
    {
        bool isSprinting = IsSprinting();
        animator.SetBool(isSprintingParam, isSprinting);
        rigController.SetBool(isSprintingParam, isSprinting);
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
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

    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion* groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;

        cc.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

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

    Vector3 CalculateAirControl()
    {
        return ((transform.forward * inputVector.y) + (transform.right * inputVector.x)) * (airControl / 100);
    }


    void Jump()
    {
        if(!isJumping)
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }




    //private void Move()
    //{

    //    inputVector.x = Input.GetAxis("Horizontal");
    //    inputVector.y = Input.GetAxis("Vertical");
    //    Vector3 currentHorizontalDir = new Vector3(inputVector.x, 0.0f, inputVector.y);



    //    float inputMagnitude = inputVector.magnitude;







    //    Vector3 inputDirection = new Vector3(inputVector.x, 0.0f, inputVector.y);


    //    if (currentHorizontalDir != Vector3.zero)
    //    {
    //        _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y; 


    //        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
    //            RotationSmoothTime);



    //        if (_rotateOnMove)
    //        {
    //            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

    //        }


    //    }


    //    Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;


    //    cc.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
    //                     new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);



    //}

    bool _rotateOnMove;

    public void SetRotateOnMove(bool newRotateOnMove)
    {
        _rotateOnMove = newRotateOnMove;
    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;

public class ShootController : MonoBehaviour
{

    public float sensitivity = 1f;
    [SerializeField] private float normalSensitivity=2;
    [SerializeField] private float aimSensitivity=1;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private LayerMask camColliderLayerMask = new LayerMask();

    private Transform _mainCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    public GameObject CinemachineCameraTarget;
    public float CameraAngleOverride = 0.0f;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    private const float _threshold = 0.01f;



    ActiveWeapon activeWeapon;
    private PlayerController playerController;
    Animator animator;
    int isAimingParam = Animator.StringToHash("isAiming");


    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas firstPersonCanvas;
    [SerializeField] private Canvas aimCanvas;
   
    
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        _mainCamera = Camera.main.transform;
        animator = GetComponent<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeWeapon = GetComponent<ActiveWeapon>();

        aimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;

    }



    private void Update()
    {
        
        Aiming();     
        Shooting();
        HandleCamMode();
       
    }

   
    private void LateUpdate()
    {
        
        CamerRotation();

    }
    #region 카메라
    void CamerRotation()
    {
        
        
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        Vector2 mousePos = new Vector3(mx, my);


        if (mousePos.sqrMagnitude >= _threshold)
        {

            float deltaTimeMultiplier = 1.0f;

            _cinemachineTargetYaw += mx * deltaTimeMultiplier * sensitivity;
            _cinemachineTargetPitch += -my * deltaTimeMultiplier * sensitivity;
        }


        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
        if (isCamModing)
        {
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, 30);
        }

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);

    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    #endregion
    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }


    

    #region 캠모드
    //bool isCamModing()
    //{
    //    bool isCamModing = Input.GetKeyDown(KeyCode.V);
    //    bool isFireing = activeWeapon.isFiring();
    //    return isCamModing && !isFireing;
    //}

    bool isCamModing;
    public Image camCrosshair;


    void HandleCamMode()
    {
        bool tryCamMode = Input.GetKeyDown(KeyCode.V);
        if (tryCamMode && !isCamModing)
        {
            isCamModing = true;

        }
        
        else if(isCamModing)
        {
            aimCanvas.enabled = false;
            thirdPersonCanvas.enabled = false;
            virtualCamera.gameObject.SetActive(true);
           
            
            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, camColliderLayerMask))
            {

                mouseWorldPosition = raycastHit.point;
                if(raycastHit.transform.gameObject.CompareTag("KeyItemDetectArea"))
                {
                    camCrosshair.color = new Color(1f, 0.8f, 0f, 1f);
                }
                else if(raycastHit.transform.gameObject.CompareTag("KeyItem"))
                {
                    camCrosshair.color = new Color(1f, 0f, 0f, 1f);
                }
                
                
            }
            else camCrosshair.color = new Color(1f, 1f, 1f, 1f);

            
            SetSensitivity(aimSensitivity);
            
            

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            if (tryCamMode)
            {
                thirdPersonCanvas.enabled = true ;
                virtualCamera.gameObject.SetActive(false);
                SetSensitivity(normalSensitivity);
                
                isCamModing = false;
            }


        }

    }
    //public List<GameObject> target = new List<GameObject>();
    //public CinemachineVirtualCamera cam;
    #endregion
    //bool isSecondVisible(CinemachineVirtualCamera c, GameObject target)
    //{
    //    var planes =GeometryUtility.CalculateFrustumPlanes;
    //    var point = target.transform.position;

    //    foreach(var plane in planes)
    //    {
    //        if(plane.GetDistanceToPoint(point)<0)
    //        {
    //            return false;
    //        }

    //    }
    //    return true;
    //}


    
    public float aimDuration = 0.3f;

    void Aiming()
    {
        bool isAiming = Input.GetMouseButton(1);
        animator.SetBool(isAimingParam, isAiming);
        if (isAiming)
        {
            aimCanvas.enabled = true;
            thirdPersonCanvas.enabled = false;
            LookForward();
            SetSensitivity(aimSensitivity);
            playerController.SetRotateOnMove(false);
           
            
        }

        else
        {
            aimCanvas.enabled = false;
            thirdPersonCanvas.enabled = true;
            SetSensitivity(normalSensitivity);
            playerController.SetRotateOnMove(true);
            

        }

    }

    void Shooting()
    {
        bool isShooting = Input.GetMouseButton(0);
        if (isShooting)
        {
            LookForward();
            playerController.SetRotateOnMove(false);

        }

        else
        {
            playerController.SetRotateOnMove(true);
        }
    }


    void LookForward()
    {
       
            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {

                mouseWorldPosition = raycastHit.point;

            }
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        
    }

    
    

}


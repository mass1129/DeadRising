using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CarEnterExitSystem : MonoBehaviour
{
    public MonoBehaviour carcontroller;
    public MonoBehaviour attackFlame;

    Transform Player;
    public Transform Car;
    public GameObject detact;
    public GameObject audioListener;

    [SerializeField] private CinemachineFreeLook carCam;
    [Header("SFX")]
    [SerializeField] AudioClip[] driveAudioClip;
    public AudioSource[] audioSource;
    
    //public GameObject driveUI;

    bool candrive;
    public bool driving;
    ActiveWeapon activeWeapon;
    int activeIndex;
    int holsterIndex;

    private void Awake()
    {
       
        
    }
    void Start()
    {
        Player = GameObject.Find("Player").transform;
        detact.SetActive(false);
        audioListener.SetActive(false);
        carcontroller.enabled = false;
        carCam.enabled = false;
        
    }

   
    void Update()
    {
       
        if(Input.GetKeyDown(KeyCode.E)&& candrive&& !driving)
        {
            activeWeapon = Player.gameObject.GetComponent<ActiveWeapon>();
            StartCoroutine(StartDrive());
            
        }

        else if(Input.GetKeyDown (KeyCode.E) && driving)
        {
            audioSource[0].Stop();
            candrive = false;
            
            carCam.enabled = false;
           
            carcontroller.enabled = false;
            

            Player.transform.SetParent(null);
           
            Player.gameObject.SetActive(true);
            detact.SetActive(false);
            driving = false;
           
            audioListener.SetActive(false);


        }

        if (driving)
            CarFiringSFX();


    }

    IEnumerator StartDrive()
    {
        activeWeapon.ToggleActiveWeapon();
        yield return new WaitForSeconds(0.5f);
        audioListener.SetActive(true);
        carCam.enabled = true;

        carcontroller.enabled = true;

        Player.transform.SetParent(Car);

        Player.gameObject.SetActive(false);
        audioListener.SetActive(true);
        OnCarSFX();
        CarDrivingSFX(driveAudioClip[1]);

        detact.SetActive(true);

        driving = true;    
    }
    private void OnCarSFX()
    {

        audioSource[0].PlayOneShot(driveAudioClip[0]);
        
    }
    private void CarDrivingSFX(AudioClip clip)
    {

        audioSource[0].clip = clip;
        audioSource[0].loop = true;
        audioSource[0].volume = drivingVolume;
        audioSource[0].Play();
        

    }
    private void CarFiringSFX()
    {

        audioSource[1].clip = driveAudioClip[2];
        audioSource[1].loop = true;
        audioSource[1].volume = fireVolume;
       
        
        bool isTryFire = Input.GetButtonDown("Fire1");
        bool stopFire = Input.GetButtonUp("Fire1");

        if (isTryFire) firing = true;

        if (firing)
        {
            audioSource[1].Play(0);
            firing = false;
        }

        if (stopFire)
        {
            print("11111");
            audioSource[1].Stop();

        }


    }
    bool firing;
    void HandleFireSFX()
    {
        

    }
    public float drivingVolume = 0.1f;
    public float fireVolume = 0.5f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player" )
        {
            



            candrive = true;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            
            candrive = false;
        }
    }





}

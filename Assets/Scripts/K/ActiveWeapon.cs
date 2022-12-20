using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlot
    {
        Primary = 0,
        Secondary = 1
    }
    public Animator rigController;
    public Transform[] weaponSlots;
    public bool isChangingWeapon;

    RayCastWeapon1[] equipped_weapon = new RayCastWeapon1[2];
    public CharacterAiming characterAiming;
    public PlayerUI ammoWidget;
    public Transform crossHairTarget;


    [System.NonSerialized] public int activeWeaponIndex = -1;
    //int activeWeaponIndex;
    //����� ������Ʈ, ���Ⱑ ���� ���̾Ű ����
    bool isHolstered = false;

    

    

    [Header("Reload")]
    public WeaponAnimationEvents animationEvents;





    //���⽽�� �ѹ���


    //�������� ���� �迭 ����(2��)

    //Ȱ���� ���� �ε���


    private void Awake()
    {
        //crossHairTarget = Camera.main.transform.Find("CrossHairTarget");
        ammoWidget = FindObjectOfType<PlayerUI>();
        characterAiming = GetComponent<CharacterAiming>();
        
    }
    void Start()
    {
        rigController.updateMode = AnimatorUpdateMode.AnimatePhysics;
        rigController.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        rigController.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        rigController.updateMode = AnimatorUpdateMode.Normal;
        animationEvents.WeaponAnimationEvent.AddListener(OnAnimationEvent);
        EquipFirstWeapon();
        //�¾�� �������� ���Ⱑ �ִ��� Ž��, 
        //RayCastWeapon1 existingWeapon = GetComponentInChildren<RayCastWeapon1>();
        ////�������� ���Ⱑ �������
        //if (existingWeapon)
        //{   //�������ι��� ����
        //    Equip(existingWeapon);
        //}
    }
    public bool IsFiring()
    {
        RayCastWeapon1 currentWeapon = GetActiveWeapon();
        if (!currentWeapon) return false;
        return currentWeapon.isFiring;
    }


    public bool isReloading = false;
    void Update()
    {
        var weapon = GetWeaPon(activeWeaponIndex);
        bool notSprinting = rigController.GetCurrentAnimatorStateInfo(2).shortNameHash == Animator.StringToHash("not_sprinting");

        bool canFire = !isHolstered && notSprinting && !isReloading;// && !weapon.isFiring;
        //print(activeWeaponIndex);
        if (weapon)
        {

            if (Input.GetButton("Fire1") &&  canFire)
            {
                weapon.StartFiring();
            }
            if (Input.GetButtonUp("Fire1") || !canFire)
            {
                weapon.StopFiring();
            }

            if (Input.GetKeyDown(KeyCode.R) || weapon.ShouldReload())
            {
                isReloading = true;
                rigController.SetTrigger("reload_weapon");
            }

            weapon.UpdateWeapon(Time.deltaTime, crossHairTarget.position);

           
            ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount, activeWeaponIndex, weapon.uninfinitybullet);

            
            if (Input.GetKeyDown(KeyCode.X) && canFire)
            {
                ToggleActiveWeapon();

            }
        }

        var weapon1 = GetWeaPon(0);
        var weapon2 = GetWeaPon(1);
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapon1)
        {
           
            SetActiveWeapon(WeaponSlot.Primary);
            

        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && weapon2)
        {
            SetActiveWeapon(WeaponSlot.Secondary);
            
        }

        DownGradePrimaryWeapon(weapon1);
        UpgradeWeaponSystem();
    }

    #region ����
    void OnAnimationEvent(string eventName)
    {
        
        switch (eventName)
        {
            case "detach_magazine":
                DetachMagazine();
                break;
            case "drop_magazine":
                DropMagazine();
                break;
            case "refill_magazine":
                RefillMagazine();
                break;
            case "attach_magazine":
                AttachMagazine();
                break;
        }
        
    }
    public Transform leftHand;
    GameObject magazineHand;
    private void DetachMagazine()
    {
        RayCastWeapon1 weapon = GetActiveWeapon();
        if(weapon.isMusinGun)
        magazineHand = Instantiate(weapon.magazine, leftHand, true);
        else magazineHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);
        weapon.ReloadSFX();
    }

    private void DropMagazine()
    {
        GameObject droppedMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();
        magazineHand.SetActive(false);

       
    }
    private void RefillMagazine()
    {
        magazineHand.SetActive(true);
    }


    private void AttachMagazine()
    {
        RayCastWeapon1 weapon = GetActiveWeapon();
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);
        weapon.RefillAmmo();
        rigController.ResetTrigger("reload_weapon");
        if(ammoWidget)
        {
            ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount, activeWeaponIndex, weapon.uninfinitybullet);
        }
        isReloading = false;
    }
    #endregion


    public RayCastWeapon1 BasicPrimaryweaponFab;

    public RayCastWeapon1 UpgradePrimaryweaponFab;

    public RayCastWeapon1 MachineGunweaponFab;

    private void DownGradePrimaryWeapon(RayCastWeapon1 weapon1)
    {
        
        if (weapon1)
        {
            if (weapon1.primaryWeaponUpGrade1)
            {
                ammoWidget.arrow2Slot.SetActive(true);
                if (weapon1.clipCount < 0 )
                {
                    ammoWidget.arrow2Slot.SetActive(false);
                    EquipFirstWeapon();
                }
            }
            else ammoWidget.arrow2Slot.SetActive(false);
        }
    }

    void EquipFirstWeapon()
    {
        if (BasicPrimaryweaponFab != null)
        {
            RayCastWeapon1 newWeapon = Instantiate(BasicPrimaryweaponFab);
            Equip(newWeapon);
        }
    }    


    void UpgradeWeaponSystem()
    {   
        if (PlayerEXP.instance.upgradeWeapon)
        {
            if (PlayerEXP.instance.Level%2  ==0)
            {
                RayCastWeapon1 newWeapon = Instantiate(MachineGunweaponFab);
                Equip(newWeapon);
                PlayerEXP.instance.upgradeWeapon = false;
            }

            if (PlayerEXP.instance.Level != 1 &&  PlayerEXP.instance.Level %2 == 1)
            {
                RayCastWeapon1 newWeapon = Instantiate(UpgradePrimaryweaponFab);
                Equip(newWeapon);
                PlayerEXP.instance.upgradeWeapon = false;
            }

        }
    }



    private void OnEnable()
    {   
        
        rigController.updateMode = AnimatorUpdateMode.AnimatePhysics;
        rigController.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        rigController.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        rigController.updateMode = AnimatorUpdateMode.Normal;
        

    }
    private void OnDisable()
    {
        

    }


    public RayCastWeapon1 GetActiveWeapon()
    {
        return GetWeaPon(activeWeaponIndex);
    }

    //���⸦ ��� �Լ�
    RayCastWeapon1 GetWeaPon(int index)
    {   //���� �ε����� 0���� �۰ų� �迭�� ũ�⺸�� Ŭ��� ��ȯ�� ����
        if (index < 0 || index >= equipped_weapon.Length)
            return null;
        //�׷��� ������� �ش� �ε����� �迭 ��ġ�� ���� ���� ����. 
        return equipped_weapon[index];
    }


    
        //���� �����ϴ� �Լ�,�� �Լ��� ������ ��ũ��Ʈ�� ���ڰ����� �޴´�.
        public void Equip(RayCastWeapon1 newWeapon)
        {
            //���� ������ ���� ���������� ������ �����Ѵ�.
            int weaponSlotIndex = (int)newWeapon.weaponSlot;
            //�ش� �������� �迭 ��ġ�� ���⸦ �����Ѵ�.
            var weapon = GetWeaPon(weaponSlotIndex);
            //���Ⱑ �̹� �ش� �迭�� ���� ���
            if (weapon)
            {
                //�ش� ���⸦ �����Ѵ�.
                Destroy(weapon.gameObject);

            }
            weapon = newWeapon;
        
        
            weapon.recoil.characterAiming = characterAiming;
            weapon.recoil.rigController = rigController;
            weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
            equipped_weapon[weaponSlotIndex] = weapon;
            

            SetActiveWeapon(newWeapon.weaponSlot);

        }

    public void OnEnabeEquip()
    {

        for (int i = 0; i < 2; i++)
        {
            var weapon = GetWeaPon(i);
            if (weapon)
            {
                weapon.recoil.characterAiming = characterAiming;
                weapon.recoil.rigController = rigController;
                weapon.transform.SetParent(weaponSlots[i], false);
                equipped_weapon[i] = weapon;
            }

        }
        
    }



    public bool canDrive = false;
    public void ToggleActiveWeapon()
    {   
        ammoWidget.DeactiveSlotUI();
        StartCoroutine(HolsterWeapon(activeWeaponIndex));
        activeWeaponIndex = -1;
        canDrive = true;
    }
    


    void SetActiveWeapon(WeaponSlot weaponSlot)
    {
        int holsterIndex = activeWeaponIndex;
        int activateIndex = (int)weaponSlot;
        
        if (holsterIndex == activateIndex || isChangingWeapon)
        {
            return;
            
        }
        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
       
    }

    public IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        rigController.SetInteger("weapon_index", activateIndex);
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));
        activeWeaponIndex = activateIndex;
    }

    IEnumerator HolsterWeapon(int index)
    {
        isChangingWeapon = true;
        isHolstered = true;
        var weapon = GetWeaPon(index);
        if (weapon)
        {

            rigController.SetBool("holster_weapon", true);
            do
            {
                yield return new WaitForSeconds(0.05f);
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            
            isChangingWeapon = false;
            
        }
        
    }

    public IEnumerator ActivateWeapon(int index)
    {   
        isChangingWeapon = true;
        var weapon = GetWeaPon(index);
        if (weapon)
        {
            ammoWidget.ActiveSlotUI(index);
            rigController.SetBool("holster_weapon", false);
            rigController.Play("weapon_" + weapon.weaponName+"_equip");
            do
            {
                yield return new WaitForSeconds(0.05f);
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            
            isHolstered = false;
            
        }
        isChangingWeapon = false;
       
    }

    //public void DropWeapon()
    //{
    //    var currentWeapon = GetActiveWeapon();
    //    if (currentWeapon)
    //    {
    //        currentWeapon.transform.SetParent(null);
    //        currentWeapon.gameObject.GetComponent<BoxCollider>().enabled = true;
    //        currentWeapon.gameObject.AddComponent<Rigidbody>();
    //        equipped_weapon[activeWeaponIndex] = null;
    //    }
    //}

    public void RefillAmmo(int clipCount)
    {
        var weapon = GetActiveWeapon();
        if (weapon)
        {
            weapon.clipCount += clipCount;
            ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount, activeWeaponIndex, weapon.uninfinitybullet);
        }
    }


}

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
    

    public Transform crossHairTarget;
    public Transform[] weaponSlots;


    RayCastWeapon1[] equipped_weapon = new RayCastWeapon1[2];
    public CharacterAiming characterAiming;
    public PlayerUI ammoWidget;

    [System.NonSerialized] public int activeWeaponIndex = -1;

    bool isHolstered = false;
    public bool isReloading = false;
    public bool isChangingWeapon;
    public bool canDrive = false;

    public RayCastWeapon1 BasicPrimaryweaponFab;

    public RayCastWeapon1 UpgradePrimaryweaponFab;

    public RayCastWeapon1 MachineGunweaponFab;


    private void Awake()
    {
        ammoWidget = FindObjectOfType<PlayerUI>();
        characterAiming = GetComponent<CharacterAiming>();
        
    }

    private void OnEnable()
    {
        rigController.updateMode = AnimatorUpdateMode.AnimatePhysics;
        rigController.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        rigController.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        rigController.updateMode = AnimatorUpdateMode.Normal;
    }

    void Start()
    {
        rigController.updateMode = AnimatorUpdateMode.AnimatePhysics;
        rigController.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        rigController.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        rigController.updateMode = AnimatorUpdateMode.Normal;

        
        EquipFirstWeapon();
    }
   
    void Update()
    {
        HandleFireWeapon();
        HandleSwapWeapon();
        UpgradeWeaponSystem();
    }

    void HandleFireWeapon()
    {
        var weapon = GetWeaPon(activeWeaponIndex);
        bool notSprinting = rigController.GetCurrentAnimatorStateInfo(2).shortNameHash == Animator.StringToHash("not_sprinting");
        bool canFire = !isHolstered && notSprinting && !isReloading;

        if (weapon)
        {

            if (Input.GetButton("Fire1") && canFire)
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



        }
    }
    void HandleSwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var weapon1 = GetWeaPon(0);
            if (weapon1 != null)
                SetActiveWeapon(WeaponSlot.Primary);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var weapon2 = GetWeaPon(1);
            if (weapon2 != null)
                SetActiveWeapon(WeaponSlot.Secondary);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(ToggleActiveWeapon());

        }

        var weapon = GetWeaPon(0);
        if(weapon!=null)
        {
            if (weapon.primaryWeaponUpGrade1)
            {
                ammoWidget.arrow2Slot.SetActive(true);
                if (weapon.clipCount <= 0&&weapon.ammoCount<=0)
                {
                    ammoWidget.arrow2Slot.SetActive(false);
                    EquipFirstWeapon();
                }
            }
            else ammoWidget.arrow2Slot.SetActive(false);
        }   
    }

    void UpgradeWeaponSystem()
    {
        if (PlayerEXP.instance.upgradeWeapon)
        {
            if (PlayerEXP.instance.Level % 2 == 0)
            {

                if (GetWeaPon(1) == null)
                {
                    RayCastWeapon1 newWeapon = Instantiate(MachineGunweaponFab);
                    Equip(newWeapon, false);
                }
                else
                {
                    RefillAmmo(GetWeaPon(1), 5);
                }
                PlayerEXP.instance.upgradeWeapon = false;
            }

            if (PlayerEXP.instance.Level != 1 && PlayerEXP.instance.Level % 2 == 1)
            {
                if (!GetWeaPon(0).primaryWeaponUpGrade1)
                {
                    RayCastWeapon1 newWeapon = Instantiate(UpgradePrimaryweaponFab);
                    Equip(newWeapon, false);
                }
                else
                {
                    RefillAmmo(GetWeaPon(0), 3);
                }
                PlayerEXP.instance.upgradeWeapon = false;
            }
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

    public bool IsFiring()
    {
        RayCastWeapon1 currentWeapon = GetActiveWeapon();
        if (!currentWeapon) return false;
        return currentWeapon.isFiring;
    }


    //Ȱ��ȭ�� ���ⰴü�� �������� �Լ�
    public RayCastWeapon1 GetActiveWeapon()
    {
        return GetWeaPon(activeWeaponIndex);
    }

    //�ش� �ε��� ���ⰴü�� �������� �Լ�
    RayCastWeapon1 GetWeaPon(int index)
    {   //���� �ε����� 0���� �۰ų� �迭�� ũ�⺸�� Ŭ��� ��ȯ�� ����
        if (index < 0 || index >= equipped_weapon.Length)
            return null;
        //�׷��� ������� �ش� �ε����� �迭 ��ġ�� ���� ���� ����. 
        return equipped_weapon[index];
    }


    
    //���� �����ϴ� �Լ�
    public void Equip(RayCastWeapon1 newWeapon, bool equipNow=true)
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
        ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount, weaponSlotIndex, weapon.uninfinitybullet);
        if (equipNow)
        SetActiveWeapon(newWeapon.weaponSlot);
    }



    public IEnumerator ToggleActiveWeapon()
    {   
        ammoWidget.DeactiveSlotUI();
        yield return StartCoroutine(HolsterWeapon(activeWeaponIndex));
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

    public void RefillAmmo(int clipCount)
    {
        var weapon = GetActiveWeapon();
        if (weapon)
        {
            weapon.clipCount += clipCount;
            ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount, activeWeaponIndex, weapon.uninfinitybullet);
        }
    }
    public void RefillAmmo(RayCastWeapon1 weapon ,int clipCount)
    {
        if (weapon)
        {
            weapon.clipCount += clipCount;
            ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount, activeWeaponIndex, weapon.uninfinitybullet);
        }
    }


    
}

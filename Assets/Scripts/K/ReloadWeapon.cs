using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadWeapon : MonoBehaviour
{

    ActiveWeapon activeWeapon;
    public WeaponAnimationEvents animationEvents;
    public Transform leftHand;
    GameObject magazineHand;

    void Start()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
        animationEvents.WeaponAnimationEvent.AddListener(OnAnimationEvent);
    }

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
    RayCastWeapon1 weapon;
    private void DetachMagazine()
    {
        weapon = activeWeapon.GetActiveWeapon();
        if ((int)weapon.type == 2)
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
       
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);
        weapon.RefillAmmo();
        activeWeapon.rigController.ResetTrigger("reload_weapon");
        if (activeWeapon.ammoWidget)
        {
            activeWeapon.ammoWidget.Refresh(weapon.ammoCount, weapon.clipCount, activeWeapon.activeWeaponIndex, weapon.uninfinitybullet);
        }
        activeWeapon.isReloading = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public RayCastWeapon1 weaponFab;


    private void OnTriggerEnter(Collider other)
    {   
        //�ε���������Ʈ���� �ش� ��ũ��Ʈ�� �����´�.
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        //�ش� ��ũ��Ʈ�� �������
        if(activeWeapon)
        {
            var currentWeapon = activeWeapon.GetActiveWeapon();
            if (currentWeapon.weaponSlot == weaponFab.weaponSlot && currentWeapon.type != weaponFab.type && activeWeapon.isReloading)
                return;
            RayCastWeapon1 newWeapon = Instantiate(weaponFab);
            activeWeapon.Equip(newWeapon);
        }
    }
}

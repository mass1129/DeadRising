using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public RayCastWeapon1 weaponFab;


    private void OnTriggerEnter(Collider other)
    {   
        //부딪힌오브젝트에서 해당 스크립트를 가져온다.
        ActiveWeapon activeWeapon = other.gameObject.GetComponent<ActiveWeapon>();
        //해당 스크립트가 있을경우
        if(activeWeapon)
        {   
            //
            RayCastWeapon1 newWeapon = Instantiate(weaponFab);
            activeWeapon.Equip(newWeapon);
        }
    }
}

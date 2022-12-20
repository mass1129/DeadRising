using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    JH_Ragdoll ragdoll;
    UIHealthBar healthBar;
    protected override void OnStart()
    {
        healthBar = GetComponentInChildren<UIHealthBar>();
        ragdoll = GetComponent<JH_Ragdoll>();
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);

    }
    protected override void OnDeath(Vector3 direction)
    {
        ragdoll.ActivateRagdoll();
    }
    protected override void OnDamage(Vector3 direction)
    {
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
    }
    protected override void OnUpdate(float intensity)
    {
       
    }
}

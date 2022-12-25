using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    JH_Ragdoll ragdoll;
    PlayerUI healthBar;
    protected override void OnStart()
    {
        base.OnStart();
        healthBar = GetComponentInChildren<PlayerUI>();
        ragdoll = GetComponent<JH_Ragdoll>();
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);

    }
    protected override void OnDeath(Vector3 direction)
    {
        ragdoll.ActivateRagdoll();
    }
    protected override void OnDamage(float amount, Vector3 direction)
    {   
        base.OnDamage(amount, direction);
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
    }
    protected override void OnUpdate()
    {
       
    }
}

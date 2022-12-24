using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public float blinkIntensity = 0.6f;
    public float blinkDuration = 0.3f;
    public float blinkTimer;

    public SkinnedMeshRenderer skinnedMeshRenderer;
    

    private void Start()
    {
        OnStart();
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        OnDamage(amount,direction);
    }

    public void Die(Vector3 direction)
    {
        OnDeath(direction);
    }



    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnStart()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        currentHealth = maxHealth;
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            JH_HitBox hitBox = rigidBody.gameObject.AddComponent<JH_HitBox>();
            hitBox.health = this;
        }
    }
  
    protected virtual void OnDeath(Vector3 direction)
    {

    }
    protected virtual void OnDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        if (currentHealth <= 0.0f)
        {
            Die(direction);
        }


    }
    protected virtual void OnUpdate()
    {

    }
}

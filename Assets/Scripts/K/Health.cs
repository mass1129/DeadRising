using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public float blinkIntensity = 0.6f;
    public float blinkDuration = 0.3f;
    float blinkTimer;

    SkinnedMeshRenderer skinnedMeshRenderer;
    

    




    private void Start()
    {
        
        
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();


        currentHealth = maxHealth;
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            JH_HitBox hitBox = rigidBody.gameObject.AddComponent<JH_HitBox>();
            hitBox.health = this;
            

           
        }
        OnStart();
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        OnDamage(direction);
        if (currentHealth <= 0.0f)
        {

            Die(direction);
        }

        blinkTimer = blinkDuration;
    }

    public void Die(Vector3 direction)
    {
        OnDeath(direction);
        //Destroy(gameObject, 5f);
    }



    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity);

        skinnedMeshRenderer.material.SetColor("_EmissiveColor", (Color.white * intensity));
        OnUpdate(intensity);
        //skinnedMeshRenderer.material.color = Color.white * intensity;



    }





    protected virtual void OnStart()
    {

    }
  
    protected virtual void OnDeath(Vector3 direction)
    {

    }
    protected virtual void OnDamage(Vector3 direction)
    {

    }protected virtual void OnUpdate(float intensity)
    {

    }
}

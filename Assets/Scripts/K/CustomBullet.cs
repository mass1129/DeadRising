using UnityEngine;

/// Thanks for downloading my custom bullets/projectiles script! :D
/// Feel free to use it in any project you like!
/// 
/// The code is fully commented but if you still have any questions
/// don't hesitate to write a yt comment
/// or use the #coding-problems channel of my discord server
/// 
/// Dave
public class CustomBullet : MonoBehaviour
{
    //Assignables
    
    public GameObject explosion;
    public LayerMask whatIsEnemies;
    public AudioSource explosionSFX;


    //Damage
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    //Lifetime
    [Range(0,2)]
    public float maxLifetime;
    

    
    PhysicMaterial physics_mat;
     
    JH_EnemyHealth[] healths;
    private void Start()
    {
        Setup();
        explosionSFX = GetComponent<AudioSource>();
    }
    bool isExploded;
    private void Update()
    {
        
        maxLifetime -= Time.deltaTime;
        if (maxLifetime > 0&& isEnemy)
            transform.position = hitObject.transform.position;
        if (maxLifetime <= 0 && !isExploded) 
        {
            Explode();
            isExploded = true;
        }
    }
    Vector3[] dir;
    private void Explode()
    {
        //Instantiate explosion
       
        var impact =Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
        explosionSFX.PlayOneShot(explosionSFX.clip);
        Destroy(impact, 2);
        //Check for enemies 
        //isExploded = true;
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            //print(enemies.Length);
            //Add explosion force (if enemy has a rigidbody)
            //if (enemies[i].GetComponent<Rigidbody>())
            //    enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
            
                if(enemies[i].GetComponent<JH_HitBox>())
                {
                
                    enemies[i].GetComponent<JH_HitBox>().OnExplosionHit(Vector3.up);
                }
            
                
            
            //print(healths.Length);
        }

        //Destroy(gameObject);
        Invoke("Delay", 2f);
        
        //Add a little delay, just to make sure everything works fine

    }
    private void Delay()
    {
        Destroy(gameObject);
    }

    

    private void Setup()
    {
        //Create a new Physic material
        physics_mat = new PhysicMaterial();
       
        //physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        //physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        //Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

      
    }
    bool isEnemy;
    GameObject hitObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(8))
        {
            isEnemy = true;
            hitObject = other.gameObject;

        }

    }

    /// Just to visualize the explosion range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}

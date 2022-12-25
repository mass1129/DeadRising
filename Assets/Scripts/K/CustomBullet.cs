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
    public GameObject explosion;
    public LayerMask whatIsEnemies;
    public AudioSource explosionSFX;


    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    [Range(0,2)]
    public float maxLifetime;

    bool isExploded;
    bool firstHIt;

    GameObject hitObject;

    private void Start()
    {
        explosionSFX = GetComponent<AudioSource>();
    }

    private void Update()
    {
        TimeCaculation();
    }

    void TimeCaculation()
    {
        maxLifetime -= Time.deltaTime;
        if (hitObject!=null)
            transform.position = hitObject.transform.position;
        if (maxLifetime <= 0 && !isExploded)
        {
            Explode();
            isExploded = true;
        }
    }
    private void Explode()
    {

        var impact =Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
        explosionSFX.PlayOneShot(explosionSFX.clip);
        Destroy(impact, 2);

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
                if(enemies[i].GetComponent<JH_HitBox>())
                {
                    enemies[i].GetComponent<JH_HitBox>().OnExplosionHit(Vector3.up);
                }
        }
        Invoke("Delay", 3f);
    }
    private void Delay()
    {
        Destroy(gameObject);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(8)&&!firstHIt)
        {
            firstHIt = true;
            hitObject = other.gameObject;

        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}

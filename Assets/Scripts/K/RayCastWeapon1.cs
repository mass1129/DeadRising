using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RayCastWeapon1 : MonoBehaviour
{
    class Bullet
    {
        
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
        public GameObject tracerSmoke;
        //public ParticleSystem projectileEffect;

    }
    [Header("Bullet")]
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    List<Bullet> bullets = new List<Bullet>();
    float maxLifeTime = 3.0f;
    public GameObject explosionArrow;

    [Header("Weapon")]
    public ActiveWeapon.WeaponSlot weaponSlot;
    public bool isFiring = false;
    public int fireRate = 25;

    public GameObject magazine;
    public int ammoCount = 30;
    public int clipSize=30;
    public int clipCount=2;
    public float damage = 10;
    public WeaponRecoil recoil;
    [SerializeField] private LayerMask shootColliderLayerMask = new LayerMask();
    public bool uninfinitybullet;

    [Header("Effect")]
    public ParticleSystem[] muzzleFlash;
    public Transform raycastOrigin;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect1;
    public GameObject tracerSmoke;

    [Header("WeaponType")]
    public bool primaryWeaponUpGrade1;
    public bool isMusinGun;
    public string weaponName;

    [Header("SFX")]
    [SerializeField] AudioClip[] fireAudioClip;
    [SerializeField] AudioClip reloadAudioClip;
    private AudioSource audioSource;





    Ray ray;
    RaycastHit hitinfo;
    float accumulatedTime;

    void Awake()
    {
        recoil =GetComponent<WeaponRecoil>();
        audioSource = GetComponent<AudioSource>();
    }
    Vector3 GetPosition(Bullet bullet)
    {

        //�Ѿ� �߷�
        Vector3 gravity = Vector3.down * bulletDrop;
        //�Ѿ��� ó�� ��ġ���� time��ŭ �̵��� �Ÿ��� ���ϰ� �װ��� ��ȯ�Ѵ�.
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    //�Ѿ� ����
    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        //�Ҹ��� �����Ѵ�.
        Bullet bullet = new Bullet();
        //�߻���ġ ����
        bullet.initialPosition = position;
        //�߻�� �ʹ� �Ŀ�
        bullet.initialVelocity = velocity;
        //�ð� 0�ʷ� ����
        bullet.time = 0.0f;
        //�Ѿ˿� ���� ����, �߻���ġ�� ����, wwwwwww
        bullet.tracer = Instantiate(tracerEffect1, position, Quaternion.identity);
        bullet.tracerSmoke = Instantiate(tracerSmoke,raycastOrigin.transform.position, Quaternion.identity);
        bullet.tracerSmoke.transform.forward = velocity;
        if(bullet.tracerSmoke != null)
        Destroy(bullet.tracerSmoke.gameObject, 2f);

        //ó����ġ���� �̵��� ��Ŵ
        bullet.tracer.AddPosition(position);

        //���� �Ӽ��� ���� �Ѿ��� ��ȯ��.
        return bullet;
    }

   
    public void StartFiring()
    {
        //�߻���
        isFiring = true;
        if(accumulatedTime > 0.0f)
        accumulatedTime = 0.0f;

        recoil.Reset();
        
    }

    public void UpdateWeapon(float deltaTime, Vector3 target)
    {
        
        if (isFiring)
        {
            UpdateFiring(deltaTime, target);
        }
        accumulatedTime += deltaTime;

        UpdateBullets(deltaTime);
        

    }
    public void UpdateFiring(float deltaTime, Vector3 target)
    {

        //�߻� ����
        float fireInterval = 1.0f / fireRate;
        //������ 0���� Ŭ �� �ݺ�.
        while (accumulatedTime>=0.0f)
        {
            //�Ѿ� �߻�
            FireBullet(target);
            //������ �߻� ���ݸ�ŭ ��.��� ���ٰ� 0���� �۾����� �ݺ����� ����ɰ���.
            accumulatedTime -= fireInterval;
        }
        
        
    }
    //update�� �ش� �޼ҵ� ���� -> �Ѿ��� ��� ������Ʈ ���ٰ���.
    public void UpdateBullets(float deltaTime)
    {
        SimulateBullet(deltaTime);
        DestroyBullets();
    }

    void SimulateBullet(float deltaTime)
    {
        //�Ѿ� ����
        bullets.ForEach(bullet =>
        {
            //p0��ġ ����
            Vector3 p0 = GetPosition(bullet);
            //�Ѿ� time ����. -> �̰����� ���� p0���� p1���� �̵��ϰ� ?.
            bullet.time += deltaTime;
            //p1��ġ ����
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    void DestroyBullets()
    {
        //�Ҹ�����(�Ҹ�time�� maxtime�� �Ѿ��)
        bullets.RemoveAll(bullet => bullet.time >= maxLifeTime);
    }
    
    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        //���� ����
        Vector3 direction = end - start;
        //�ش� ���� ������ ũ�⸦ ��ȯ
        float distance = direction.magnitude;
        //ray�������� start.
        ray.origin = start;
        //ray�� ���� ����.
        ray.direction = direction;
        //���̸� ��� ���� ��ü�� �Ÿ��� ��ȯ�ϰ� ��ȯ�� ������ ���� ��.
        if (Physics.Raycast(ray, out hitinfo, distance, shootColliderLayerMask))
        {
            //hit����Ʈ�� �߻���Ų��
            hitEffect.transform.position = hitinfo.point;
            hitEffect.transform.forward = hitinfo.normal;
            hitEffect.Emit(1);
            
            //�Ҹ��� �ð��� �ִ�ð����� ������ ��������� �Ѵ�.
            bullet.time = maxLifeTime;
            end = hitinfo.point;

            if (primaryWeaponUpGrade1) Instantiate(explosionArrow, hitinfo.point, Quaternion.identity);

            var rb2d = hitinfo.collider.GetComponent<Rigidbody>();
            if (rb2d)
            {
                
                    if(!primaryWeaponUpGrade1)
                    rb2d.AddForceAtPosition(ray.direction * 20, hitinfo.point, ForceMode.Impulse);
                
            }

            var hitBox = hitinfo.collider.GetComponent<JH_HitBox>();
            if (hitBox)
            {
                hitBox.OnRaycastHit(this, ray.direction);
            }

            var headBox = hitinfo.collider.GetComponent<JH_isHeadHit>();
            if (headBox)
            {   
                if(!primaryWeaponUpGrade1)
                headBox.OnRaycastHeadHit(ray.direction);
            }
            if (bullet.tracerSmoke != null && !primaryWeaponUpGrade1)
                Destroy(bullet.tracerSmoke.gameObject);
        }


        //�Ѿ� ������ ��ġ�� ���̰� ���� ��ġ�� ������ �Ѵ�. trailrenderer�̱� ������ ������ �׷������̴�.
        if(bullet.tracer)
        bullet.tracer.transform.position = end;

       
    }
   

    

    private void FireBullet(Vector3 target)
    {   
        if(ammoCount <=0) return;

        ammoCount--;
        if(!isMusinGun) magazine.SetActive(false);
        //�ѱ� ����Ʈ ����.
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
            
        }
        
        //�߻��Ŀ� ����.
        Vector3 velocity = (target - raycastOrigin.position).normalized * bulletSpeed;
       
        //�Ҹ��� ����(�߻���ġ, �߻��Ŀ� ����.)
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        //�Ҹ� ����Ʈ�� �Ҹ� �߰�.
        bullets.Add(bullet);
        PlayFireSFX();
        recoil.GenerateRecoil(weaponName);
    }


    public void StopFiring()
    {
        isFiring=false;
    }

    private void PlayFireSFX()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);

    }
    public void ReloadSFX()
    {
        AudioClip clip = reloadAudioClip;
        audioSource.PlayOneShot(clip);

    }
    private AudioClip GetRandomClip()
    {
        int index = Random.Range(0, fireAudioClip.Length - 1);
        return fireAudioClip[index];
    }


    public bool ShouldReload()
    {
        return ammoCount <= 0 && clipCount > 0;
    }

    public bool IsLowAmmo()
    {
        return ammoCount == 0 && clipCount <= 0;
    }
    public void RefillAmmo()
    {
        ammoCount = clipSize;
        clipCount--;
    }


}

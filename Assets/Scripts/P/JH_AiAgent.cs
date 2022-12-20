using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JH_AiAgent : MonoBehaviour
{
    public JH_AiStateMachine stateMachine;
    public NavMeshAgent navMeshAgent;
    public JH_AiAgentConfig config;
    public JH_Ragdoll ragdoll;
    public GameObject playerObject;
    public Animator anim;
    public GameObject agentObject;
    public int attackDamage = 10;
    public int killExp;
    [SerializeField] AudioClip[] zombieAudioClip;
    public AudioSource[] audioSource;

    // Start is called before the first frame update
    void Start()
    {
        agentObject = gameObject;
        playerObject = GameObject.Find("Player");
        if(!playerObject){
            GameObject.Find("Car");
        }
        ragdoll = GetComponent<JH_Ragdoll>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        stateMachine = new JH_AiStateMachine(this);
        stateMachine.RegisterState(new JH_AiChasePlayerState());
        stateMachine.RegisterState(new JH_AiDeathState());
        stateMachine.RegisterState(new JH_AiAttackState());
        stateMachine.RegisterState(new JH_AiRoamingState());

        int randomRotate = Random.Range(0,360);
        gameObject.transform.rotation = Random.rotation;

        stateMachine.ChangeState(AiStateId.Roaming);

        
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    public float zombie2Player;  
    public void Attack()
    {
        var hitBox =  playerObject.GetComponentInChildren<JH_HitBox>();
        if(zombie2Player<= navMeshAgent.stoppingDistance && hitBox)
        {
            hitBox.OnZombieAttack(this, transform.forward);
        }

    }

    public void OneShotSFX(int index)
    {

        audioSource[0].PlayOneShot(zombieAudioClip[index]);

    }
    public float loopVolume = 0.5f;
    public void LoopSFX(int index)
    {

        audioSource[1].clip = zombieAudioClip[index];
        audioSource[1].loop = true;
        audioSource[1].volume = loopVolume;
        audioSource[1].Play();
    }

    public void StopLoopSFX(int index)
    {

        audioSource[1].clip = zombieAudioClip[index];
        audioSource[1].Stop();
    }


}

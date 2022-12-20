using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class JH_AiRoamingState : JH_AiState
{
    public Vector3 direction;

    public AiStateId GetId(){
        return AiStateId.Roaming;
    }

    public float speed_NonCombat;
    public int moveRange;

    bool onMove;
    Vector3 originPosition;
    float originDis;
    Vector3 target;
    float targetDis;

    Transform enemyTransform;
    NavMeshAgent nav;

    GameObject playerTarget;

    float randomAnimSpeed;

    float playerDistance;

    Scene currentScene;
    string sceneName;


    public void Enter(JH_AiAgent agent){
        agent.LoopSFX(0);
        originPosition = agent.transform.position;
        
        enemyTransform = agent.transform;
        nav = agent.navMeshAgent;

        speed_NonCombat = agent.config.roamingSpeed;
        moveRange = agent.config.roamingRange;

        randomAnimSpeed = Random.Range(0.5f,1.5f);

        agent.navMeshAgent.SetDestination(target);

        playerTarget = GameObject.Find("Player");

        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;

        
    }
    void Move_NonCombat(){
        if(targetDis <= 3f){
            onMove = false;
            nav.speed = speed_NonCombat;
        }
        if(!onMove){
            onMove = true;
            target = new Vector3(enemyTransform.position.x + Random.Range(-1 * moveRange, moveRange), 0, enemyTransform.position.z + Random.Range(-1 * moveRange, moveRange));
            nav.SetDestination(target);
        }
    if(originDis>=moveRange){
        onMove = true;
        target = originPosition;
        nav.SetDestination(target);
    }

    }

    public void Update(JH_AiAgent agent){
            originDis = (originPosition - enemyTransform.position).magnitude;
            targetDis = (target - enemyTransform.position).magnitude;

            agent.anim.speed = randomAnimSpeed;

            playerDistance = Vector3.Distance(playerTarget.transform.position, agent.transform.position);
            Move_NonCombat();

            float detactDistance;

            if(sceneName == "5_detective_Lab"){
                detactDistance = 7f;
                agent.config.maxDistance = 7f;
            }else if(sceneName == "1_opening"){
                detactDistance = 1f;
                agent.config.maxDistance = 1f;
            }else{
                detactDistance = 15f;
                agent.config.maxDistance = 15f;
            }
            

            if(playerDistance <= detactDistance){
                agent.anim.SetTrigger("Detact");
                agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            }



    }

    public void Exit(JH_AiAgent agent){
        
        agent.StopLoopSFX(0);
    }
}


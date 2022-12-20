using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_AiAttackState : JH_AiState
{
    float randomAinm;
    float playerDistance;
   
    // Start is called before the first frame update
    public AiStateId GetId(){
        return AiStateId.Attack;
    }

    public void Enter(JH_AiAgent agent){
        
        
        randomAinm = Random.Range(0,2);
        agent.anim.SetFloat("AttackAction",randomAinm);
    }

    public void Update(JH_AiAgent agent){
        playerDistance = Vector3.Distance(agent.playerObject.transform.position, agent.transform.position);
        if(playerDistance >= agent.navMeshAgent.stoppingDistance){
            agent.anim.SetTrigger("Detact");
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }

        if(agent.playerObject){
            agent.transform.LookAt(agent.playerObject.transform.position);
            agent.zombie2Player = playerDistance;
        }
    }

    public void Exit(JH_AiAgent agent){
        
    }

    

}

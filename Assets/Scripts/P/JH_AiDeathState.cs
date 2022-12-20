using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_AiDeathState : JH_AiState
{
    public Vector3 direction;
    bool expUp = false;
    public AiStateId GetId(){
        return AiStateId.Death;
    }

    public void Enter(JH_AiAgent agent){
        agent.StopLoopSFX(1);
        agent.OneShotSFX(3);
        agent.ragdoll.ActivateRagdoll();
        direction.y = 1;
        agent.ragdoll.ApplyForce(direction * agent.config.dieForce);
        agent.navMeshAgent.enabled = false;
        if (!expUp)
        {
            PlayerEXP.instance.KillNum += agent.killExp;
            expUp = true;
        }
        GameObject.Destroy(agent.agentObject,5f);
    }

    public void Update(JH_AiAgent agent){
        
    }

    public void Exit(JH_AiAgent agent){
        agent.StopLoopSFX(1);
    }
}

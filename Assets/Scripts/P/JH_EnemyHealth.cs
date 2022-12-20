using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_EnemyHealth : Health
{
    JH_AiAgent agent;

    public GameObject head;
    public GameObject body;
    public GameObject headBlink;
    
    SkinnedMeshRenderer skinnedMeshRendererHead;

    protected override void OnStart()
    {
        agent = GetComponent<JH_AiAgent>();
        skinnedMeshRendererHead = headBlink.GetComponent<SkinnedMeshRenderer>();
        JH_isHeadHit headHit = head.GetComponent<JH_isHeadHit>();
        headHit.health = this;
        
        
    }
    protected override void OnDeath(Vector3 direction)
    {
        
        JH_AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as JH_AiDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AiStateId.Death);
    }
    protected override void OnDamage(Vector3 direction)
    {

    }
    protected override void OnUpdate(float intensity)
    {
        if (skinnedMeshRendererHead)
        {
            skinnedMeshRendererHead.material.SetColor("_EmissiveColor", (Color.white * intensity));
        }
    }

}
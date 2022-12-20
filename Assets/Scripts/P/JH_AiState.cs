using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateId{
    ChasePlayer,
    Death,
    Attack,
    Roaming
}

public interface JH_AiState
{
    AiStateId GetId();
    void Enter(JH_AiAgent agent);
    void Update(JH_AiAgent agent);
    void Exit(JH_AiAgent agent);
}

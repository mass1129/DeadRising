using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_AiStateMachine
{
    public JH_AiState[] states;
    public JH_AiAgent agent;
    public AiStateId currentState;

    public JH_AiStateMachine(JH_AiAgent agent){
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AiStateId)).Length;
        states = new JH_AiState[numStates];
    }

    public void RegisterState(JH_AiState state){
        int index = (int)state.GetId();
        states[index] = state;
    }

    public JH_AiState GetState(AiStateId stateId){
        int index = (int)stateId;
        return states[index];
    }

    public void Update(){
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(AiStateId newState){
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
    }
}

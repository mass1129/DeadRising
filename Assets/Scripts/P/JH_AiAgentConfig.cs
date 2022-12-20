using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class JH_AiAgentConfig : ScriptableObject
{
    public float maxDistance = 1.0f;
    public float dieForce = 1f;
    public float maxSightDistance = 5.0f;
    public int roamingRange = 5;
    public float roamingSpeed = 0.5f;
    
}

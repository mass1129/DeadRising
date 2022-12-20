using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JH_EventTrigger : MonoBehaviour
{
    public GameObject enemyGroup;
    bool active = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !enemyGroup.activeInHierarchy && !active)
        {
            enemyGroup.SetActive(true);
            active = true;
        }
        else if (other.CompareTag("Player") && enemyGroup.activeInHierarchy && !active)
        {
            enemyGroup.SetActive(false);
            active = true;
        }
    }
}

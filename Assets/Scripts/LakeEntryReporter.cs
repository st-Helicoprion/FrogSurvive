using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeEntryReporter : MonoBehaviour
{
    private void Start()
    {
        FishEnemyMovement.targetLake = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FishEnemyMovement.targetLake = this.transform;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FishEnemyMovement.targetLake = null;
           
            
        }
    }
}

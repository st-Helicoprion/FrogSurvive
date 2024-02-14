using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameReporter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerStateManager.win = true;
        }
    }
}

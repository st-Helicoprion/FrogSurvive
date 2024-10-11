using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundKissingReporter : MonoBehaviour
{
    public Action OnFacePlant;
    public Action OnSwitchedToClimb;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            OnFacePlant?.Invoke();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            OnSwitchedToClimb?.Invoke();
        }
    }

}

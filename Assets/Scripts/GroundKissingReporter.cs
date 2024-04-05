using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundKissingReporter : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            PlayerMovement.facePlant = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            PlayerMovement.facePlant = false;
        }
        
    }

}

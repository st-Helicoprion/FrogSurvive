using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectTriggerReporter : MonoBehaviour
{
    public InsectBehavior behavior;

    public void DisableInsect()
    {
        behavior.gameObject.SetActive(false);
    }

    public void SaveBehaviorToBuffer()
    {
        behavior.CheckInsectType();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Lake"))
        {
            DisableInsect();
            
        }
    }
}

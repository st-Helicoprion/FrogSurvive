using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Radio: MonoBehaviour
{
    [SerializeField] private RadioContent radioContent;
    [SerializeField] private bool isSeen;

    private void SendContent()
    {
        if (radioContent != null)
        {
            StoryManager.instance.OnRadioFound?.Invoke(radioContent);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")&&!isSeen)
        {
            isSeen = true;
            SendContent();
        }
    }
}

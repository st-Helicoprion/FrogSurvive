using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<OwlEnemyMovement>(out _))
        {
            return;
        }
        else
        {
            if (other.TryGetComponent(out Rigidbody rb))
            {
                StartCoroutine(GradualSpeedDecrease(rb));
            }
        }
    }

    IEnumerator GradualSpeedDecrease(Rigidbody rb)
    {
        while(rb.velocity.y>0)
        {
            rb.velocity -= new Vector3(0, 2, 0);
            yield return null;
        }
        StartCoroutine(SendBackDown(rb));
    }

    IEnumerator SendBackDown(Rigidbody rb)
    {
        int increment = 3;

        while(increment>0)
        {
            increment--;
            rb.velocity -= new Vector3(0, 0.1f, 0.1f);
            yield return null;
        }
    }
}

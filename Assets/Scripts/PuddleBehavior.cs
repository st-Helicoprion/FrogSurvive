using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class PuddleBehavior : MonoBehaviour
{
    public LineRenderer beaconLine;
    public bool isGrounded;
    public float puddleLifetime, healAmount;

    private void Update()
    {
        if (!isGrounded)
            transform.position += new Vector3(0, 0.25f, 0);

        if (puddleLifetime>0)
        {
            puddleLifetime -= Time.deltaTime;
        }
        else
        {
            puddleLifetime = 0;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            isGrounded= true;
           
        }

        if(other.CompareTag("Sonar"))
        {
            beaconLine.enabled = true;
        }
    }
}

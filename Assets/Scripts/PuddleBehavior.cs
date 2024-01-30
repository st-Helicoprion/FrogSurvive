using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class PuddleBehavior : MonoBehaviour
{
    public GameObject beaconLine;
    public bool isGrounded, isFound;
    public float healAmount;

    private void Update()
    {
        if (!isGrounded)
            transform.position += new Vector3(0, 0.15f, 0);
    }

    void MarkPuddle()
    {
        beaconLine.SetActive(true);
        isFound = true;
        
    }

    public void DisablePuddle()
    {
        gameObject.SetActive(false);
        beaconLine.SetActive(false);
        isFound = false;
        isGrounded= false;
    }

    void CheckPuddleClipping()
    {
        if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y-0.2f, transform.position.z), Vector3.up, out RaycastHit hit,Mathf.Infinity))
        {
            if (hit.transform.CompareTag("Ground") || hit.transform.CompareTag("Puddle"))
            {
                float randX = Random.Range(-5, 5);
                float randZ = Random.Range(-5, 5);
                transform.position += new Vector3(randX, 0.25f, randZ);

            }
            else return;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            isGrounded = true;
            CheckPuddleClipping();
        }

        if(other.CompareTag("Lake"))
        {
           DisablePuddle();
        }

        if(other.CompareTag("Sonar"))
        {
            MarkPuddle();
        }
    }
}

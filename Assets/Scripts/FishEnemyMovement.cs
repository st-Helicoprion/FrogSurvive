using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishEnemyMovement : MonoBehaviour
{
    public Transform playerPos, fishHead;
    public Rigidbody[] rbArray;
    public float moveSpeed, airTime,
                 attackInterval, attackCountdown,
                 swimInterval, swimCountdown,
                 locateInterval, locateCountdown;
    public static bool recoverAfterAttack, alert, isUnderwater;
    public Transform[] lakeMap;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGravityMultiplier();
    }

    IEnumerator RespondToSonar()
    {
        float moveToPlayerCount = 4;
        while (moveToPlayerCount > 0)
        {

            print("detected");
            moveToPlayerCount--;

            fishHead.LookAt(playerPos);
            Vector3 direction = playerPos.position - transform.position;
            transform.forward = direction;
            rbArray[0].AddForce(0.01f * moveSpeed * direction);
            yield return null;
        }

    }

    void CheckGravityMultiplier()
    {
        for(int i =0; i < rbArray.Length; i++)
        {
            if(isUnderwater)
            {
                rbArray[i].drag = 2.5f;
            }
            else
            {
                if (rbArray[i].drag > 0)
                {
                    rbArray[i].drag -= 0.05f;
                }
                else rbArray[i].drag = 0;
            }

            if(!isUnderwater)
            {
                airTime += Time.deltaTime;

                if (airTime > 3)
                {
                    airTime = 0;
                    rbArray[i].useGravity = true;
                    rbArray[i].AddForce(0, -0.5f* moveSpeed, 0.5f* moveSpeed);
                }
            }
        }
    }

    IEnumerator SwitchToUnderwater()
    {
        yield return new WaitForSeconds(2);
        for(int i = 0;i < rbArray.Length; i++)
        {
            rbArray[i].useGravity = false;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sonar"))
        {
            StartCoroutine(RespondToSonar());
        }

        if (other.CompareTag("Lake"))
        {
            isUnderwater = true;
            StartCoroutine(SwitchToUnderwater());
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Lake"))
        {
            isUnderwater = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lake"))
        {
            isUnderwater = false;
        }
    }

}

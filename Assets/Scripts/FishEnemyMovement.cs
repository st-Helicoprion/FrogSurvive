using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class FishEnemyMovement : MonoBehaviour
{
    public Transform playerPos, fishHead, currentLake;
    public static Transform targetLake;
    public Rigidbody[] rbArray;
    public float moveSpeed, airTime,
                 attackInterval, attackCountdown,
                 swimInterval, swimCountdown,
                 locateInterval, locateCountdown;
    public static bool recoverAfterAttack, alert, 
                       isUnderwater, isBeached;
    public Transform[] lakeMap;
    public int orientation;
    public Vector3 newRotation;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!AppUtilsManager.isPaused)
        {
            CheckGravityMultiplier();
            if (isBeached)
            {
                locateInterval = 10;
            }
            else locateInterval = 60;

            if (recoverAfterAttack)
            {
                recoverAfterAttack = false;
                StartCoroutine(RecoverAfterAttack());
            }
            else
            {
                swimCountdown -= Time.deltaTime;

                if(swimCountdown<0)
                {
                    swimCountdown = swimInterval;
                    StartCoroutine(FishAnimation());
                }

            }

            if(currentLake == targetLake&&!alert)
            {
                alert = true;
            }

            if (alert)
            {
                SwitchToAlert();

                attackCountdown -= Time.deltaTime;

                if(attackCountdown<0)
                {
                    AttackPlayer();
                }
            }
            else
            {

                locateCountdown -= Time.deltaTime;

                if (locateCountdown < 0)
                {
                    locateCountdown = locateInterval;
                    SwitchLakesJump();
                }

                rbArray[0].AddForce(moveSpeed * transform.forward);
            }

          
        }
       
    }

    void SwitchLakesJump()
    {
        if(targetLake==null)
        {
            int lakeNum = Random.Range(0, 3);
            StartCoroutine(JumpSequence(lakeMap[lakeNum].position - transform.position));

        }
        else
        {
            StartCoroutine(JumpSequence(targetLake.position));
        }

    }

    IEnumerator JumpSequence(Vector3 targetLake)
    {
        if(isUnderwater)
        {
            rbArray[0].AddForce(1000 * moveSpeed * Vector3.up);
        }
        else rbArray[0].AddForce(350 * moveSpeed * Vector3.up);
        yield return new WaitForSeconds(1);
        transform.LookAt(targetLake);
        rbArray[0].AddForce(0.1f*moveSpeed * targetLake);

    }

    void SwitchToAlert()
    {
        transform.LookAt(playerPos);
       
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

    void AttackPlayer()
    {
        attackCountdown = attackInterval;
        Vector3 direction = playerPos.position - transform.position;
        rbArray[0].AddForce(20 * moveSpeed * direction);
    }

    IEnumerator RecoverAfterAttack()
    {
        
        yield return null;
        alert = false;

        orientation = Random.Range(0, 2);

        if (orientation == 0)
        {
            newRotation.y += 90;
            transform.localRotation = Quaternion.Euler(newRotation);
        }

        if (orientation == 1)
        {
            newRotation.y -= 90;
            transform.localRotation = Quaternion.Euler(newRotation);
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

    IEnumerator FishAnimation()
    {
        rbArray[1].AddForce(5*moveSpeed*transform.right);
        rbArray[3].AddForce(-5*moveSpeed*transform.right);
        yield return new WaitForSeconds(1);
        rbArray[1].AddForce(-5*moveSpeed * transform.right);
        rbArray[3].AddForce(5*moveSpeed * transform.right);
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
            currentLake = other.transform;
        }

       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lake"))
        {
            isUnderwater = false;
        }
       
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Ground") && !isUnderwater)
        {
            isBeached = true;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground") && !isUnderwater)
        {
            isBeached = false;
        }
    }
}

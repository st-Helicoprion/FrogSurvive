using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemyMovement : MonoBehaviour
{
    public Transform playerPos;
    public Rigidbody[] rbArray;
    public float huntRadius, moveSpeed, slitherOffset, airTime,
                 attackInterval, attackCountdown,
                 slitherInterval, slitherCountdown,
                 locateInterval, locateCountdown;
    public static bool recoverAfterAttack, alert, isGrounded, isUnderwater;
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
       
        rbArray[12].AddForce(2 * moveSpeed * Vector3.down);
        rbArray[16].AddForce(2 * moveSpeed * Vector3.down);
        CheckBody();
       
        if (recoverAfterAttack)
        {
            recoverAfterAttack = false;
            StartCoroutine(RecoverAfterAttack());
        }
        else
        {
            slitherCountdown -= Time.deltaTime;

            if (slitherCountdown < 0)
            {
                slitherCountdown = slitherInterval;
                StartCoroutine(SlitherAnimation());
            }
        }

        if (Vector3.Distance(transform.position, playerPos.position) < huntRadius && !alert)
        {
            alert = true;
        }

        if (alert)
        {
            SwitchToAlert();
            attackCountdown -= Time.deltaTime;

            if (attackCountdown < 0)
            {
                AttackPlayer();
            }
        }
        else
        {
            CheckGravityMultiplier();
            GroundSticking();

            rbArray[0].AddForce(6 * moveSpeed * transform.forward);

            locateCountdown -= Time.deltaTime;

            if(locateCountdown<0)
            {
                MoveToPlayer();
            }
        }
    }

    void GroundSticking()
    {
        if (!isGrounded&&!alert)
        {
            rbArray[0].AddForce(4 * moveSpeed * Vector3.down);
           
        }
        else return;
        
    }

    void MoveToPlayer()
    {
        locateCountdown = locateInterval;
        transform.LookAt(playerPos);
    }
    void AttackPlayer()
    {
        attackCountdown = attackInterval;
        Vector3 direction = playerPos.position - transform.position;

        rbArray[0].AddForce(10*moveSpeed*direction);
        rbArray[8].AddForce(10 * moveSpeed * Vector3.down);
        rbArray[0].useGravity = true;

    }

    void SwitchToAlert()
    {
       
        transform.LookAt(playerPos);
        rbArray[0].useGravity = false;
        rbArray[0].AddForce(3*moveSpeed*Vector3.up);
        rbArray[6].AddForce(2*moveSpeed * Vector3.down);

        for(int i=7;i<rbArray.Length;i++)
        {
            rbArray[i].mass = 500;
        }
    }
    IEnumerator RecoverAfterAttack()
    {
        rbArray[0].AddForce(10 * moveSpeed * -transform.forward);
        yield return new WaitForSeconds(2);
        alert = false;
        for (int i = 7; i < rbArray.Length; i++)
        {
            rbArray[i].mass = 100;
        }

        orientation = Random.Range(0, 5);

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

    void CheckBody()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.transform.name == "SnakeBody")
            {
               
                rbArray[0].AddForce(4*moveSpeed * Vector3.up);
            }
            else return;
        }
    }
    IEnumerator RespondToSonar()
    {
        float moveToPlayerCount = 4;
        while (moveToPlayerCount > 0)
        {
            moveToPlayerCount--;

            transform.LookAt(playerPos);
            Vector3 direction = playerPos.position - transform.position;
            transform.forward = direction;
            rbArray[0].AddForce(0.01f * moveSpeed * direction);
            yield return null;
        }

    }
    void CheckGravityMultiplier()
    {
        if (isGrounded || isUnderwater)
        {
            for(int i =0; i<rbArray.Length;i++)
            {
                rbArray[i].drag = 3;
            }
        }
        else
        {
            for (int i = 0; i < rbArray.Length; i++)
            {
                if (rbArray[i].drag > 0)
                {
                    rbArray[i].drag -= 0.05f;
                }
                else rbArray[i].drag = 0;
            }
           

        }

        if (!isGrounded && !isUnderwater)
        {
            airTime += Time.deltaTime;

            if (airTime > 3)
            {
                airTime = 0;
                for (int i = 0; i < rbArray.Length; i++)
                {
                    rbArray[i].useGravity = true;
                    rbArray[0].AddForce(0, -2*moveSpeed, 0.5f*moveSpeed);
                    transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, transform.localRotation.z);
                }
            }
        }
    }

    IEnumerator SwitchToUnderwater()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < rbArray.Length; i++)
        {
            rbArray[i].useGravity = false;
        }
     
    }
    IEnumerator SlitherAnimation()
    {

        rbArray[1].AddForce(moveSpeed * slitherOffset * (transform.right));
        yield return new WaitForSeconds(1);
        rbArray[1].AddForce(moveSpeed * slitherOffset * (-transform.right));
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sonar"))
        {
            StartCoroutine(RespondToSonar());
        }

        if(other.CompareTag("Lake"))
        {
            isUnderwater = true;
            StartCoroutine(SwitchToUnderwater());
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
        if(collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

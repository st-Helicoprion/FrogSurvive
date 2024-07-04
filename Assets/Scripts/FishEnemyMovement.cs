using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
                 swimInterval, swimCountdown;
    public static bool recoverAfterAttack, alert, 
                       isUnderwater;

    public int orientation;
    public Vector3 newRotation;

    [Header("Audio")]
    public AudioSource fishAudioSource;
    public AudioClip attackAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
        currentLake = null;
        targetLake = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(!AppUtilsManager.isPaused)
        {
            CheckGravityMultiplier();
            
            if (recoverAfterAttack)
            {
                recoverAfterAttack = false;
                StartCoroutine(RecoverAfterAttack());
            }
            else
            {
                swimCountdown -= Time.deltaTime;

                if (swimCountdown < 0)
                {
                    swimCountdown = swimInterval;
                    StartCoroutine(FishAnimation());
                }

            }

            if (currentLake == targetLake&&targetLake!=null)
            {
                alert = true;
            }
            else
            {
                alert = false;
                attackCountdown = attackInterval;
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

                rbArray[0].AddForce(moveSpeed * transform.forward);


            }


        }
       
    }


    void SwitchToAlert()
    {
        transform.LookAt(playerPos);
        
        
    }

    void AttackPlayer()
    {
        attackCountdown = attackInterval;
        fishAudioSource.spatialBlend = 0.5f;
        fishAudioSource.pitch = Random.Range(0.6f, 0.9f);
        fishAudioSource.PlayOneShot(attackAudioClip);
        float rand = Random.Range(-10, 10);
        Vector3 offset = new(rand, rand, rand);
        Vector3 direction = (playerPos.position+offset) - transform.position;
        rbArray[0].AddForce(20 * moveSpeed * direction);
    }

    IEnumerator RecoverAfterAttack()
    {
        
        yield return null;
        alert = false;

        orientation = Random.Range(0, 2);

        if (orientation == 0)
        {
            StartCoroutine(TurnCoroutine(90));
        }

        if (orientation == 1)
        {
            StartCoroutine(TurnCoroutine(-90));
        }

    }

    IEnumerator TurnCoroutine(int turnAmount)
    {   
        if(turnAmount>0)
        {
            while (newRotation.y<turnAmount&&!alert)
            {
                newRotation.y += .5f;
                newRotation.x = transform.localRotation.x;
                newRotation.z = transform.localRotation.z;
                transform.localRotation = Quaternion.Euler(newRotation);
                yield return null;
            }
        }
        else if(turnAmount<0)
        {
            while (newRotation.y > turnAmount&&!alert)
            {
                newRotation.y -= .5f;
                newRotation.x = transform.localRotation.x;
                newRotation.z = transform.localRotation.z;
                transform.localRotation = Quaternion.Euler(newRotation);
                yield return null;
            }
        }
    }
       
    void CheckGravityMultiplier()
    {
         if(isUnderwater)
            {
                for (int i = 0; i < rbArray.Length; i++)
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

            if(!isUnderwater)
            {
                airTime += Time.deltaTime;

                if (airTime > 1)
                {
                    
                    for (int i = 0; i < rbArray.Length; i++)
                    {
                        rbArray[i].useGravity = true;
                        rbArray[0].AddForce(airTime*moveSpeed*Vector3.down);
                   
                }
            }
            }
        
    }

    IEnumerator FishAnimation()
    {
        rbArray[1].AddForce(14*moveSpeed*transform.right);
        rbArray[3].AddForce(-56*moveSpeed*transform.right);
        yield return new WaitForSeconds(2);
        rbArray[1].AddForce(-14*moveSpeed * transform.right);
        rbArray[3].AddForce(56*moveSpeed * transform.right);
    }

    IEnumerator SwitchToUnderwater()
    {
        yield return new WaitForSeconds(2);
        for(int i = 0;i < rbArray.Length; i++)
        {
            rbArray[i].useGravity = false;
            rbArray[i].drag = 3;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Lake"))
        {
            isUnderwater = true;
            airTime = 0;
            StartCoroutine(SwitchToUnderwater());
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Lake"))
        {
            isUnderwater = true;
            airTime = 0;
            currentLake = other.transform;
        }

       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lake"))
        {
            isUnderwater = false;
            currentLake = null;
        }
       
    }

}

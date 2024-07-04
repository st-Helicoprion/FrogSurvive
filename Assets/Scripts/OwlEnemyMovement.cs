using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlEnemyMovement : MonoBehaviour
{
    public Transform playerPos, owlHead;
    public float huntRadius, moveSpeed, 
                 attackInterval, attackCountdown,
                 flapCountdown, flapInterval,
                 locateCountdown, locateInterval,
                 idleAudioCountdown, idleAudioInterval;
    public static bool recoverAfterAttack, alert;
    public Rigidbody rb;
    public Rigidbody[] wingRb;
    public static Vector3 hitAltitude;
    public PuddleRandomizer waterMap;
    public Transform[] lakeMap;
    public Vector2 huntRadiusRange;

    [Header("Audio")]
    public AudioSource owlAudioSource;
    public AudioClip attackAudioClip;
    public AudioClip[] idleAudioClip;


    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
        waterMap = FindObjectOfType<PuddleRandomizer>();
    }

    // Update is called once per frame
    void Update()
    {
       if(!AppUtilsManager.isPaused)
        {
            if (recoverAfterAttack)
            {
                recoverAfterAttack = false;
                StartCoroutine(RecoverAfterAttack());
            }
            else
            {

                flapCountdown -= Time.deltaTime;

                if (flapCountdown < 0)
                {
                    flapCountdown = flapInterval;
                    StartCoroutine(OwlWingAnimation());

                }
            }

            if (Vector3.Distance(transform.position, playerPos.position) < huntRadius && !alert)
            {
                alert = true;
                owlAudioSource.spatialBlend = 0.2f;
            }
            else owlAudioSource.spatialBlend = 0.5f;

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

                owlHead.localRotation = Quaternion.Euler(0, 0, 0);
                locateCountdown -= Time.deltaTime;
                idleAudioCountdown -= Time.deltaTime;
                if (locateCountdown < 0)
                {
                    locateCountdown = locateInterval;
                    LocateWater();
                }
                if (idleAudioCountdown < 0 && !owlAudioSource.isPlaying)
                {
                    PlayIdleAudio();
                }
            }

            if (Vector3.Distance(transform.position, playerPos.position) > 150)
            {
                huntRadius = huntRadiusRange.y;
                owlAudioSource.spatialBlend = 1;
            }

        }



    }


    IEnumerator RespondToSonar()
    {
        float moveToPlayerCount = 4;
        while(moveToPlayerCount>0)
        {
           
            print("detected");
            moveToPlayerCount--;

            owlHead.LookAt(playerPos);
            Vector3 direction = playerPos.position - transform.position;
            transform.forward = direction;
           
            rb.AddForce(0.01f*moveSpeed * direction);
            yield return null;
        }
       
    }

    void AttackPlayer()
    {
        attackCountdown = attackInterval;
        owlAudioSource.spatialBlend = 0.2f;
        owlAudioSource.pitch = Random.Range(1, 1.3f);
        owlAudioSource.PlayOneShot(attackAudioClip);
        float rand = Random.Range(-10, 10);
        Vector3 offset = new(rand, rand, rand);
        Vector3 direction = (playerPos.position+offset) - transform.position;
        transform.forward = direction;
      
        rb.AddForce(1.5f*moveSpeed * direction);

    
    }

    void FlyUpwards()
    {
        owlHead.localRotation = Quaternion.Euler(-80,0,0);    
        rb.AddForce(0.5f*moveSpeed*Vector3.up);
      
    }

    IEnumerator RecoverAfterAttack()
    {
        yield return new WaitForSeconds(3);
        while(transform.localPosition.y<hitAltitude.y+5)
        { 
            FlyUpwards();
            yield return null;
        }
        owlHead.localRotation = Quaternion.Euler(0, 0, 0);
        alert = false;
        huntRadius = huntRadiusRange.x;
    }

    IEnumerator OwlWingAnimation()
    {

        wingRb[0].AddForce(moveSpeed * (transform.forward+(-transform.right)));
        wingRb[1].AddForce(moveSpeed * (transform.forward + transform.right));
        yield return new WaitForSeconds(0.75f);
        wingRb[0].AddForce(moveSpeed * (-transform.forward + transform.right));
        wingRb[1].AddForce(moveSpeed * (-transform.forward + (-transform.right)));


    }

    void LocateWater()
    {
        float destinationID = Random.Range(0, 2);

        if(destinationID == 0)
        {
            Vector3 destination = waterMap.puddles[Random.Range(0, waterMap.puddles.Length)].transform.position;
            StartCoroutine(MoveToWater(destination));
        }
        else
        {
            Vector3 destination = lakeMap[Random.Range(0, lakeMap.Length)].position;
            StartCoroutine(MoveToWater(destination));
        }
        
    }

    IEnumerator MoveToWater(Vector3 destination)
    {
        float randX = Random.Range(-10, 11);
        float randZ = Random.Range(-10, 11);

        while(transform.position!=destination+new Vector3(randX,transform.position.y,randZ))
        {
            if (!alert)
            {
                Vector3 direction = destination - transform.position;
                owlHead.localRotation = Quaternion.Euler(-80, 0, 0);
                transform.localRotation = Quaternion.Euler(90, 0, Vector3.Angle(transform.localPosition,destination));
                
                rb.AddForce(0.001f * moveSpeed * direction);
                yield return null;
            }
            else yield break;
            
        }
        yield break;
        
    }

    void SwitchToAlert()
    {
        owlHead.LookAt(playerPos);
        transform.forward = playerPos.position;
    }

    void PlayIdleAudio()
    {
        idleAudioCountdown = idleAudioInterval;
        owlAudioSource.spatialBlend = 0.5f;
        owlAudioSource.pitch = Random.Range(0.7f, 1);
        owlAudioSource.PlayOneShot(idleAudioClip[Random.Range(0, idleAudioClip.Length)]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Sonar"))
        {
            StartCoroutine(RespondToSonar());
        }
    }

}

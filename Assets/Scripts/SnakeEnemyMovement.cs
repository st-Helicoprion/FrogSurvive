using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeEnemyMovement : MonoBehaviour
{
    public Transform playerPos, snakeHead;
    public Rigidbody rb, tailRb;
    public float moveSpeed, slitherOffset,
                 attackInterval, attackCountdown,
                 slitherInterval, slitherCountdown,
                 locateInterval, locateCountdown;
    public static bool recoverAfterAttack, alert;
    public PuddleRandomizer waterMap;
    public Transform[] lakeMap;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
        waterMap = FindObjectOfType<PuddleRandomizer>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundSticking();
    }

    void GroundSticking()
    {
        rb.AddForce(4*moveSpeed * transform.forward);
        rb.AddForce(moveSpeed * Vector3.down);
        tailRb.AddForce(moveSpeed * Vector3.down);
       
    }

    void AnimateSnakeMovement()
    {

    }

    IEnumerator RespondToSonar()
    {
        float moveToPlayerCount = 4;
        while (moveToPlayerCount > 0)
        {

            print("detected");
            moveToPlayerCount--;

            snakeHead.LookAt(playerPos);
            Vector3 direction = playerPos.position - transform.position;
            transform.forward = direction;
            rb.AddForce(0.01f * moveSpeed * direction);
            yield return null;
        }

    }

    IEnumerator SlitherAnimation()
    {
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sonar"))
        {
            StartCoroutine(RespondToSonar());
        }
    }
    }

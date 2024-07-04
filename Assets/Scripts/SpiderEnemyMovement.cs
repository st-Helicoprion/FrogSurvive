using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemyMovement : MonoBehaviour
{
    public Transform anchorTower, spider;
    public float huntRadius, moveSpeed, distToTower;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!AppUtilsManager.isPaused)
        {
            distToTower = Vector3.Distance(transform.position, anchorTower.position);

            //KeepWithinRangeOfTower();
            CheckGround();
            CheckWall();

            rb.AddForce(6 * moveSpeed * transform.forward);
        }
    }
    public void KeepWithinRangeOfTower()
    {
        if (distToTower > 700)
        {
            Vector3 offset = new Vector3(1,0,1);
            transform.forward = anchorTower.position;
        }
    }
    public void CheckGround()
    {
        if (!Physics.Raycast(transform.position, -transform.up, 10))
        {
            rb.AddForce(12 * moveSpeed * -transform.up);
            Vector3 rotAmount = new Vector3(1, 0, 0);
            transform.eulerAngles += rotAmount;
        }
        else return;

    }

    public void CheckWall()
    {
        if (Physics.Raycast(transform.position, transform.forward, 10))
        {
            Vector3 rotAmount = new Vector3(1, 0, 0);
            transform.eulerAngles -= rotAmount;
        }
        else return;
    }

    public void CheckSides()
    {

    }
}

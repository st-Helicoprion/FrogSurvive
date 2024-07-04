using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PuddleRandomizer : MonoBehaviour
{
    public GameObject[] puddles;
    public PuddleBehavior[] puddleScripts;
    public float timeToRandomize, randomizeCountdown;
    public Transform playerPos, goalPos;
    public Vector3 goalDir;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Array.Resize(ref puddles,transform.childCount);
            Array.Resize(ref puddleScripts,puddles.Length);
            puddles[i] = transform.GetChild(i).gameObject;
            puddleScripts[i] = puddles[i].GetComponent<PuddleBehavior>();
        }

        StartCoroutine(RandomizePuddlePositions());
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (randomizeCountdown>0)
        {
            randomizeCountdown -= Time.deltaTime;

        }
        else
        {
            StartCoroutine(RandomizePuddlePositions());
           
        }
    }

    void ReadyAllPuddles(int puddleIndex)
    {
       
            puddleScripts[puddleIndex].beaconLine.SetActive(false);
            puddleScripts[puddleIndex].isGrounded = false;
            puddleScripts[puddleIndex].isFound = false;
            puddles[puddleIndex].SetActive(true);
        
    }
    
   IEnumerator RandomizePuddlePositions()
    {
        randomizeCountdown = timeToRandomize;
        
        for (int i = 0; i < puddles.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            float randX = UnityEngine.Random.Range(0, 561); 
            float randZ = UnityEngine.Random.Range(0, 561);
            
            if (!puddleScripts[i].isFound)
            {
                puddles[i].transform.position = new Vector3(playerPos.position.x, 0, playerPos.position.z) + new Vector3(randX, -2, randZ);
                ReadyAllPuddles(i);
            }
            else puddleScripts[i].isFound = false;
        }

    }
}

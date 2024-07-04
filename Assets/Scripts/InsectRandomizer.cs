using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InsectRandomizer : MonoBehaviour
{
    public GameObject[] insectGroup;
    public InsectBehavior[] insectScripts;
    public float timeToRandomize, randomizeCountdown;
    public Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Array.Resize(ref insectGroup,transform.childCount);
            Array.Resize(ref insectScripts,insectGroup.Length);
            insectGroup[i] = transform.GetChild(i).gameObject;
            insectScripts[i] = insectGroup[i].GetComponent<InsectBehavior>();
        }

        StartCoroutine(RandomizeInsectPositions());
        
    }

    // Update is called once per frame
    void Update()
    {
        if(randomizeCountdown>0)
        {
            randomizeCountdown -= Time.deltaTime;

        }
        else
        {
            StartCoroutine(RandomizeInsectPositions());
           
        }
    }

    void ReadyAllInsectsInGroup(int insectIndex)
    {

        insectGroup[insectIndex].SetActive(true);
        
    }
    
   IEnumerator RandomizeInsectPositions()
    {
        randomizeCountdown = timeToRandomize;
        
        for (int i = 0; i < insectGroup.Length; i++)
        {
            yield return new WaitForSeconds(0.5f);
            float randX = UnityEngine.Random.Range(0, 561); 
            float randZ = UnityEngine.Random.Range(0, 561);
            insectGroup[i].transform.position = playerPos.position + new Vector3(randX, 0, randZ);
            insectGroup[i].transform.GetChild(0).localPosition = Vector3.zero;
            ReadyAllInsectsInGroup(i);
        }

    }
}

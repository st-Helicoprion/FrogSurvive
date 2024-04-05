using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InsectBehavior : MonoBehaviour
{
    public enum InsectType { Worms, Firefly, Beetle, BabySpider};

    public InsectType insectType;

    public int behaviorColorID, behaviorNameID;

    public void CheckInsectType()
    {
        if(insectType == InsectType.Firefly)
        {
            behaviorColorID = 1;
            behaviorNameID = 1;
        }
        else if(insectType==InsectType.Worms)
        {
            behaviorColorID = 2;
            behaviorNameID = 2;
           
        }
        else if(insectType==InsectType.BabySpider)
        {
            behaviorColorID = 3;
            behaviorNameID = 3;
        }
        else if(insectType==InsectType.Beetle)
        {
            behaviorColorID = 4;
            behaviorNameID = 4;
        }
        
    }


}

using DigitalRuby.RainMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    public static bool isRaining;
    public GameObject rain;
    public float rainCheckInterval, rainCheckCountdown, rainLifetime;
    public int rainCheck;
   
    // Update is called once per frame
    void Update()
    {
        rainCheckCountdown -= Time.deltaTime;

        if(rainCheckCountdown < 0 )
        {
            rainCheckCountdown = rainCheckInterval;
            RollForRain();
        }

        if(isRaining)
        {
            rainLifetime -= Time.deltaTime;
        }

        if(rainLifetime<0)
        {
            StopRain();
        }
    }

    void RollForRain()
    {
        rainCheck = Random.Range(0, 2);
        rainLifetime= Random.Range(30,241);

        if(rainCheck==0)
        {
            StopRain();
        }
        if(rainCheck==1)
        {
            StartRain();
        }
    }

    void StartRain()
    {
        isRaining = true;
        rain.SetActive(true);
    }

    void StopRain()
    {
        isRaining = false;
        rain.SetActive(false);
        rainLifetime= 0;
    }
}

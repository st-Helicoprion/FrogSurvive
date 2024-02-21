using DigitalRuby.RainMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    public static bool isRaining;
    public RainScript rain;
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
        rainCheck = Random.Range(0, 11);
        rainLifetime= Random.Range(30,241);

        if (rainCheck == 1)
        {
            StartRain();
        }
        else StopRain();
    }

    void StartRain()
    {
        isRaining = true;
        rain.RainIntensity=0.5f;
    }

    void StopRain()
    {
        isRaining = false;
        rain.RainIntensity=0;
        rainLifetime= 0;
    }
}

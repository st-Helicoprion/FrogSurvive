using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSonarManager : MonoBehaviour
{
    public int sonarIndex;
    public Slider sonarSlider;
    public Image sonarSliderGraphic;
    public Sprite[] sonarStageGraphic;
    public float sonarCooldown, sonarCounter;
    public GameObject[] sonars;
    public float[] sonarRanges;

    // Start is called before the first frame update
    void Start()
    {
        sonarCounter = sonarCooldown;
        sonarSliderGraphic.enabled = false;
        sonarSlider.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        SonarLifeCycle();

        if(sonarSlider.value==1)
        {
            ReleaseSonar();
        }
    }

    void SonarLifeCycle()
    {
        sonarCounter -= Time.deltaTime;

        if(sonarCounter<0&&sonarIndex<2)
        {
              IncreaseSonarIndex();
              sonarCounter = sonarCooldown;
        }

        if(sonarIndex==2)
        {
            sonarCounter = sonarCooldown;
        }
    }

    void ReleaseSonar()
    {
        for(int i = 0;i<=sonarIndex;i++)
        {
            StartCoroutine(IncreaseSonarSize(i));
            DisableSonarSlider();
        }

        sonarIndex = -1;
    }

    IEnumerator IncreaseSonarSize(int sonarIndex)
    {
        while (sonars[sonarIndex].transform.localScale.x < sonarRanges[sonarIndex])
        {
            sonars[sonarIndex].transform.localScale += new Vector3(0.5f, 0.5f, 0.5f)*(sonarIndex+1);
            Color sonarColor = sonars[sonarIndex].GetComponent<Renderer>().material.color;
            
                sonarColor.a -= 0.0025f;
           
            sonars[sonarIndex].GetComponent<Renderer>().material.color = sonarColor;
            yield return null;
        }
    }

    void FadeOutSonar(int sonarIndex)
    { 
       sonars[sonarIndex].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        Color sonarColor = sonars[sonarIndex].GetComponent<Renderer>().material.color;
        sonarColor.a = 1;
        sonars[sonarIndex].GetComponent<Renderer>().material.color = sonarColor;
    }

    void DisableSonarSlider()
    {
        sonarSlider.value= 0;
        sonarSlider.interactable= false;
        sonarSliderGraphic.enabled= false;
        sonarCounter = sonarCooldown;

    }

    void IncreaseSonarIndex()
    {
        sonarIndex++;
        sonarSlider.interactable= true;
        sonarSliderGraphic.sprite = sonarStageGraphic[sonarIndex];
        sonarSliderGraphic.enabled= true;

        FadeOutSonar(sonarIndex);

    }
}

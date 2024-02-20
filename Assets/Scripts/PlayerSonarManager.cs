using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSonarManager : MonoBehaviour
{
    public Transform player;
    public int sonarIndex;
    public Slider sonarSlider;
    public Image sonarSliderGraphic;
    public Sprite[] sonarStageGraphic;
    public float sonarCooldown, sonarCounter;
    public GameObject[] sonars;
    public float[] sonarRanges;
    public AudioSource sonarAudioSource;
  

    // Start is called before the first frame update
    void Start()
    {
        sonarCounter = sonarCooldown;
        sonarSliderGraphic.gameObject.SetActive(false);
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

        if(Input.GetKeyDown(KeyCode.Joystick1Button5)|| Input.GetKeyDown(KeyCode.Joystick1Button4))
        {
            if(sonarIndex>-1)
            {
                StartCoroutine(SonarSliderAnimation());
            }
            
        }
    }

    void SonarLifeCycle()
    {

        sonarCounter -= Time.deltaTime;

        if(sonarCounter<0&&sonarIndex<2)
        {
              IncreaseSonarIndex();
              
        }

        if(sonarIndex ==2)
        {
            sonarCounter = sonarCooldown;
        }
    }

    void ReleaseSonar()
    {
        PlayerMovement playerMove = player.GetComponent<PlayerMovement>();
        playerMove.rb.AddForce(50*(sonarIndex+1) * playerMove.moveSpeed * Vector3.up);
        sonarAudioSource.pitch = 2;
        sonarAudioSource.volume = 0.5f;
        sonarAudioSource.Play();
        for (int i = 0;i<=sonarIndex;i++)
        {
            sonars[i].transform.position = player.position;
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
            if(sonarColor.a > 0)
            {
                sonarColor.a -= 0.007f;
            }
            sonars[sonarIndex].GetComponent<Renderer>().material.color = sonarColor;
            yield return null;

        } yield return null;
        ReloadSonar(sonarIndex);
    }

    void ReloadSonar(int sonarIndex)
    {
        
        sonars[sonarIndex].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        Color sonarColor = sonars[sonarIndex].GetComponent<Renderer>().material.color;
        sonarColor.a = 1;
        sonars[sonarIndex].GetComponent<Renderer>().material.color = sonarColor;
    }

    void DisableSonarSlider()
    {
        sonarSlider.value= 0;
        sonarSlider.interactable= false;
        sonarSliderGraphic.gameObject.SetActive(false);
        sonarCounter = sonarCooldown;
      
    }

    public void IncreaseSonarIndex()
    {
        sonarIndex++;
        sonarCounter = sonarCooldown;
        sonarSlider.interactable= true;
        sonarSliderGraphic.sprite = sonarStageGraphic[sonarIndex];
        sonarSliderGraphic.gameObject.SetActive(true);

        ReloadSonar(sonarIndex);

    }

    IEnumerator SonarSliderAnimation()
    {
        while(sonarSlider.value<1)
        {
            sonarSlider.value += 12.5f*Time.deltaTime;
            yield return null;
        }
        sonarSlider.value= 0;
       

    }
}

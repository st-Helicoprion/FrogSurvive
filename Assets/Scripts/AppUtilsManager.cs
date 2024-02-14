using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppUtilsManager : MonoBehaviour
{
    public static bool isPaused, enterDeath;
    public GameObject settingsCanvas, startButton, UILayer,
                      settingsFirst, deathFirst;
    public AudioSource UIAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        PauseGame();
        enterDeath = false;
        
    }

    // Update is called once per frame
    void Update()
    {  
        
        if(PlayerStateManager.isDead)
        {
            if(!enterDeath)
            {
                enterDeath= true;
               ShowDeath();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            {
                UIAudioSource.Play();
                if (!isPaused)
                {
                    PauseGame();
                }
                else ResumeGame();
            }
        }


    }

    public IEnumerator RestartButtonAnimations(TextMeshProUGUI text)
    {
        Color animTextColor = text.color;
        while(text.characterSpacing<100)
        {
            text.characterSpacing += 150*Time.unscaledDeltaTime;
            animTextColor.a -= 1.5f*Time.unscaledDeltaTime;
            text.color = animTextColor;
            yield return null;
        }

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

    }

    public IEnumerator ExitButtonAnimations(TextMeshProUGUI text)
    {
        Color animTextColor = text.color;
        while (text.characterSpacing < 100)
        {
            text.characterSpacing += 150 * Time.unscaledDeltaTime;
            animTextColor.a -= 1.5f * Time.unscaledDeltaTime;
            text.color = animTextColor;
            yield return null;
        }
        Application.Quit();


    }

    public IEnumerator StartButtonAnimations(TextMeshProUGUI text)
    {
        Color animTextColor = text.color;
        while (text.characterSpacing < 100)
        {
            text.characterSpacing += 150 * Time.unscaledDeltaTime;
            animTextColor.a -= 1.5f * Time.unscaledDeltaTime;
            text.color = animTextColor;
            yield return null;
        }
        ResumeGame();
        text.characterSpacing= 0;
        animTextColor.a = 1;
        text.color = animTextColor;


    }
    public void ExitApp(TextMeshProUGUI text)
    {
       StartCoroutine(ExitButtonAnimations(text));
    }

    public void ResetGame(TextMeshProUGUI text)
    {
        StartCoroutine(RestartButtonAnimations(text));
        
    }

    public void StartGame(TextMeshProUGUI text)
    {
        StartCoroutine (StartButtonAnimations(text));
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale= 0;
        settingsCanvas.SetActive(true);
        startButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsFirst);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale= 1;
        settingsCanvas.SetActive(false);
    }

    void ShowDeath()
    {
        isPaused = true;
        Time.timeScale = 0;
        UILayer.SetActive(false);
        settingsCanvas.SetActive(true);
        startButton.SetActive(false);
        EventSystem.current.SetSelectedGameObject(deathFirst);
    }
    private void OnApplicationPause(bool pause)
    {
        PauseGame();
    }



}

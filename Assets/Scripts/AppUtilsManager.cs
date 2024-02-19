using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AppUtilsManager : MonoBehaviour
{
    public static bool isPaused, enterDeath, UIFocused;
    public GameObject settingsCanvas, UILayer,
                      settingsFirst, deathFirst;
    public AudioSource UIAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        PauseGame();
        enterDeath = false;
        UIFocused= false;
    }

    // Update is called once per frame
    void Update()
    {  
        
        if(PlayerStateManager.isDead)
        {
            if(!enterDeath)
            {
                enterDeath= true;
                UIFocused = false;
               ShowDeath();
            }

            if (Input.GetAxisRaw("Debug Vertical") != 0 && !UIFocused)
            {
                UIFocused = true;
                EventSystem.current.SetSelectedGameObject(deathFirst);
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

            if(isPaused)
            {
                if (Input.GetAxisRaw("Debug Vertical") != 0 && !UIFocused)
                {
                    UIFocused = true;
                    EventSystem.current.SetSelectedGameObject(settingsFirst);
                }
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
        settingsFirst.SetActive(true);
       
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale= 1;
        RestartUI();
        settingsCanvas.SetActive(false);
    }

    void ShowDeath()
    {
        isPaused = true;
        Time.timeScale = 0;
        DeathHideUI();
        settingsCanvas.SetActive(true);
        settingsFirst.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    void DeathHideUI()
    {
        UILayer.transform.GetChild(0).gameObject.SetActive(false);
        UILayer.transform.GetChild(1).gameObject.SetActive(false);
        UILayer.transform.GetChild(3).gameObject.SetActive(false);
        UILayer.transform.GetChild(4).gameObject.SetActive(false);
    }

    void RestartUI()
    {
        UILayer.transform.GetChild(0).gameObject.SetActive(true);
        UILayer.transform.GetChild(1).gameObject.SetActive(true);
        UILayer.transform.GetChild(3).gameObject.SetActive(true);
        UILayer.transform.GetChild(4).gameObject.SetActive(true);
    }

    private void OnApplicationPause(bool pause)
    {
        PauseGame();
    }



}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppUtilsManager : MonoBehaviour
{
    public bool isPaused;
    public GameObject settingsCanvas;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {  

        if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
           
            if(!isPaused)
            {
                PauseGame();
            }
            else ResumeGame();
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
    public void ExitApp()
    {
        Application.Quit();
    }

    public void ResetGame(TextMeshProUGUI text)
    {
        StartCoroutine(RestartButtonAnimations(text));
        
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale= 0;
        settingsCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale= 1;
        settingsCanvas.SetActive(false);
    }
}
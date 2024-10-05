using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Globalization;
using UnityEngine.UI;
using UnityEditor;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;

    public Action<RadioContent> OnRadioFound;
    public Action OnTypeFinished;

    private RadioContent content;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueLine;
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private GameObject continuePrompt;
    [SerializeField] private Button clickSpace;
    [SerializeField] private float speed;
    [SerializeField] private bool next;
    [SerializeField] private bool endReached;
    [SerializeField] private int currentReadIndex;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);

        OnRadioFound += SetContentToRead;
        OnTypeFinished += CheckEndReached;
        
        clickSpace.onClick.AddListener(NextLine);
    }

    private void SetContentToRead(RadioContent contentToRead)
    {
        content= contentToRead;
        OpenDialogPanel(true);
        currentReadIndex = 0;
        GetNextLine();
        AppUtilsManager.isPaused = true;
        Time.timeScale = 0;
    }
    private void OpenDialogPanel(bool open)
    {
        dialogueCanvas.SetActive(open);
    }

    private void ShowContinuePrompt(bool show)
    {
        continuePrompt.SetActive(show);
    }

    private void NextLine()
    {
        
        if(endReached)
        {
            OpenDialogPanel(false);
            AppUtilsManager.isPaused= false;
            Time.timeScale = 1;
        }
        if(next)
        {
            currentReadIndex++;

            GetNextLine();
        }
        else
        {
            next = true;  
        }
    }

    private void CheckEndReached()
    {
        if(currentReadIndex==content.lines.Length-1)
        {
            endReached= true;
        }

        ShowContinuePrompt(true);

        if(!endReached)
        {
            next = true;
            NextLine();
        }
    }

    private void GetNextLine()
    {
        next= false;
        speakerName.text = content.speaker;
        ShowContinuePrompt(false);
        if(currentReadIndex<content.lines.Length)
        StartCoroutine(TypewriteEffect(content.lines[currentReadIndex]));
    }

    IEnumerator TypewriteEffect(string sentence)
    {
        if(dialogueLine.text.Length>0)
        {
            dialogueLine.text += "\n";
        }

        int typeIndex = 0;
        string temp = ""; 

        while(typeIndex < sentence.Length&&!next)
        {
            char letter = sentence[typeIndex];
            dialogueLine.text += letter;
            temp += letter;
            typeIndex++;
            yield return new WaitForSecondsRealtime(1 / speed);
        }

        if(temp.Length > 0)
        {
            dialogueLine.text = dialogueLine.text.Replace(temp, "");
        }

        dialogueLine.text += sentence;
        OnTypeFinished?.Invoke();
    }
}

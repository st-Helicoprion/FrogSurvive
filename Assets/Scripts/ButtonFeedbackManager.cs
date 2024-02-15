using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFeedbackManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Color[] stateColors;
    public float animationTime;

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            text.color = stateColors[1];
            AnimateSelected();
        }
        else
        {
            text.color = stateColors[0];
            text.fontStyle = FontStyles.UpperCase;

        }
    }

    void AnimateSelected()
    {
        animationTime -= Time.unscaledDeltaTime;

        if(animationTime < 0 )
        {
            animationTime = 0.5f;
            if (text.fontStyle != FontStyles.Italic)
            {
                text.fontStyle = FontStyles.Italic;
            }
            else
            {
                text.fontStyle = FontStyles.UpperCase;
            }
        }
        
    }
}


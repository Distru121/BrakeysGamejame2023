using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UI_SetBestTime : MonoBehaviour
{
    //this will set the best time saved in game gestion inside a text attached to this object!
    private TMP_Text m_TextComponent;
    void Awake()
    {
        m_TextComponent = GetComponent<TMP_Text>();
        if(GameObject.Find("GlobalGameGestion") != null){
            int time = (int)Mathf.Round(GameObject.Find("GlobalGameGestion").GetComponent<GlobalGameGestion>().timetocomplete);
            if(time != -1){
                var ts = TimeSpan.FromSeconds(time);
                m_TextComponent.text = "Best Time: "+string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            }
            else
                m_TextComponent.text = "Best Time: No Time Saved!";
        }
    }
}

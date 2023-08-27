using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeIn : MonoBehaviour
{
    //this script only works on an image element in the UI
    //it enables the image, then fades it out, then re-disables it and itself
    public float fadeTime;
    Image thisImg;
    UI_FadeIn thisScript;
    // Start is called before the first frame update
    void Awake()
    {
        thisImg = GetComponent<Image>();
        thisScript = GetComponent<UI_FadeIn>();
    }

    void Update()
    {
        if(fadeTime > 0)
        {
            thisImg.enabled = true;
            thisImg.color = new Color(thisImg.color.r, thisImg.color.g, thisImg.color.b, fadeTime);
            fadeTime -= Time.deltaTime;
        }
        else
        {
            thisImg.enabled = false;
            thisScript.enabled = false;
        }
    }
}

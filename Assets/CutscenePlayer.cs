using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutscenePlayer : MonoBehaviour
{
    public Sprite[] cutscenePanels;
    public Image cutsceneView;
    public Image cutsceneFade;

    public float timeBetweenScenes = 4f;
    public float fadingTime = 1f; //this must be always lower than timebetweenscenes FOR THE LOVE OF GOD
    public int sceneIndexToGoAfterCutscene = 1;

    private float timePassed;
    private int i = 0;
    private bool isFading = false;

    // Start is called before the first frame update
    void Start()
    {
        cutsceneFade.color = new Color(1,1,1,0);
        timePassed = timeBetweenScenes;
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;

        if(timePassed > timeBetweenScenes)
        {
            if(i < cutscenePanels.Length-1){
                i++;
                cutsceneFade.sprite = cutscenePanels[i];
                isFading = true;
                timePassed = 0;
            }
            else
                ChangeScene();
        }

        if(Input.GetKeyDown(KeyCode.Return)) //can skip the cutscene if you press enter
        {
            ChangeScene();
        }


        //fade of the images: fades in if true, stops if false
        if(isFading)
        {
            cutsceneFade.color = new Color(1, 1, 1, timePassed/fadingTime);
        }
        else
        {
            cutsceneFade.color = new Color(1,1,1,0);
        }
        if(timePassed >= fadingTime)
        {
            isFading = false;
            cutsceneView.sprite = cutscenePanels[i];
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(sceneIndexToGoAfterCutscene); //the menu is the next scene in the index
    }
}

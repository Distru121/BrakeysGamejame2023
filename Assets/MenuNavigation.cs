using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public Image transitionImage;

    private bool isTransitioning = false;
    private float transitionTimer;
    private int targetSceneIndex; //if a button makes you go to another scene, this will be the index

    // Start is called before the first frame update
    void Start()
    {
        transitionImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        //transition for a new scene
        if(isTransitioning)
        {
            transitionTimer += Time.deltaTime;
            transitionImage.enabled = true;
            transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, transitionTimer);
            if(transitionTimer >= 1)
            {
                SceneManager.LoadScene(targetSceneIndex); //changes scene after fading
                isTransitioning = false;
            }
        }
    }

    public void PressPlay() {
        //fades out and changes the scene to the level 1 scene(SampleScene)
        isTransitioning = true;
        targetSceneIndex = 2;
    }

    public void PressOptions() {

    }

    public void PressTutorial() {

    }

    public void PressCredits() {
        //fades out and changes the scene to the credits scene
        isTransitioning = true;
        targetSceneIndex = 3;
    }
}
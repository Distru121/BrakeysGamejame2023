using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCredits : MonoBehaviour
{
    public int menuSceneIndex;
    public int endingCutsceneSceneIndex;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)) //can skip the credits if you press enter
        {
            BackToMenu();
        }

        if(transform.position.y <= -410) //if credits ended, return to menu
        {
            BackToMenu();
        }
    }

    void BackToMenu()
    {
        if(GameObject.Find("GlobalGameGestion")!= null)
        {
            if(GameObject.Find("GlobalGameGestion").GetComponent<GlobalGameGestion>().hasCompletedGame) {
                GameObject.Find("GlobalGameGestion").GetComponent<GlobalGameGestion>().hasCompletedGame = false;
                SceneManager.LoadScene(endingCutsceneSceneIndex);
            }
            else
                SceneManager.LoadScene(menuSceneIndex);
        }
        else
            SceneManager.LoadScene(menuSceneIndex);
    }
}
